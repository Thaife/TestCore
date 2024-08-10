using ApplicationCore.HttpService;
using ApplicationCore.Interface.Cache;
using ApplicationCore.Model;
using ApplicationCore.Service.Cache;
using ApplicationCore.Service.MiddleWare;
using ApplicationCore.Utility.Common;
using ApplicationCore.Utility.Startup;
using ApplicationCore.Web.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using ApplicationCore.LimitRate.Library;
using ConfigServiceTest;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;

namespace ApplicationCore.Web.Startup
{
    public class CoreStartup
    {
        public static void ProgramStart(IConfigurationBuilder config)
        {
            //Build config trước khi load config
            BuildConfigBeforeCreateApp();
            //Add lại config để sử dụng
            string enironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            LoadConfiguration(config, enironmentName);
            //Gán biến môi trường, ..
            config.AddEnvironmentVariables();
            //Add thêm source, ..
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            InitGlobalConfig(services, config);
            AddAuthentication(services, config);
            // Add services to the container.
            services.AddControllers();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IHttpClientStandard, HttpClientStandard>();
            services.AddSingleton<ICacheService, CacheService>();
            //Để tạm ở đây
            //
            services.ConfigureRateLimit(config);

            services.ConfiguraionSomethingService();

        }

        public static void ConfigureApp(WebApplication app)
        {
            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();
            long limit = 3;
            TimeSpan x = TimeSpan.FromHours(1);
            //app.UseMiddleware<RequestMiddleware>(x, limit);
            //app.UseMiddleware<AuthContextMiddleWare>();
            app.UseRateLimit();
        }

        public static void AddAuthentication(IServiceCollection services, IConfiguration config)
        {
            var appsettings = config.GetSection("Appsettings");
            string JwtSecretKey = appsettings["JwtSecretKey"];
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context => 
                    {
                        string a = "abc";
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        string b = "false";
                        return Task.CompletedTask;
                    }
                };
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void InitGlobalConfig(IServiceCollection services, IConfiguration config)
        {
            //Convert về model để init global
            GlobalConfig configGlobal = new GlobalConfig();
            new ConfigureFromConfigurationOptions<GlobalConfig>(config).Configure(configGlobal);
            //Khởi tạo config global cho dễ dùng
            GlobalConfigUtility.InitConfig(configGlobal);
        }

        private static void LoadConfiguration(IConfigurationBuilder config, string env)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var configMapContent = GetConfigContent("ConfigMap.json");
            if (!string.IsNullOrEmpty(configMapContent))
            {
                ConfigMap configMap = ConvertUtility.Deserialize<ConfigMap>(configMapContent);
                if (configMap.Source?.Count > 0)
                {
                    for (int i = config.Sources.Count - 1; i >= 0; i--)
                    {
                        var source = config.Sources[i];
                        if (source.GetType().Name == typeof(JsonConfigurationSource).Name)
                        {
                            config.Sources.RemoveAt(i);
                        }
                    }
                    foreach (var item in configMap.Source)
                    {
                        string fileName = $"{item}.json";
                        LoadConfigFile(ref config, fileName);
                    }
                }
            }
            else
            {
                //log không đọc được config
            }
        }

        private static void LoadConfigFile(ref IConfigurationBuilder config, string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                config.AddJsonFile(path, false, true);
            }
        }

        private static string GetConfigContent(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                string content = "";
                content = File.ReadAllText(path);
                return content;
            }
            return string.Empty;

        }

        /// <summary>
        /// Build config trước khi chạy service
        /// </summary>
        public static void BuildConfigBeforeCreateApp()
        {
            //Lấy là path của của file dll thực thi
            string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //Từ rootPath truy vấn ngược để lấy đúng folder Config/Appsettings.json
            string pathConfigCommon = GetPathConfigCommon(rootPath);
            //Từ rootPath get path của file appsetting.json và appsetting.{enviroment}.json được build ra
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string pathConfigBuilded = Path.Combine(rootPath, "appsettings.json");
            string pathConfigEnvironmentBuilded = Path.Combine(rootPath, $"appsettings.{environment}.json");
            //Merge pathConfigCommon vào pathConfigBuilded
            MergeJsonFile(pathConfigBuilded, pathConfigCommon);
            //Merge config riêng của service vào pathConfigEnvironmentBuilded
            string pathConfigService = GetPathConfig(pathConfigCommon, $"{Assembly.GetEntryAssembly().GetName().Name}.json");
            MergeJsonFile(pathConfigEnvironmentBuilded, pathConfigService);
            //Đọc config
        }
        /// <summary>
        /// Get path của file từ 1 dirctoryFile cùng cấp
        /// </summary>
        /// <param name="directoryFile">đường dẫn file</param>
        /// <param name="newFileName">tên file cùng cấp với directoryFile</param>
        /// <returns></returns>
        public static string GetPathConfig(string directoryFile, string newFileName)
        {
            if (string.IsNullOrEmpty(directoryFile) || string.IsNullOrEmpty(newFileName))
            {
                return string.Empty;
            }
            string directoryFolder = Path.GetDirectoryName(directoryFile);
            string newFilePath = Path.Combine(directoryFolder, newFileName);
            if (File.Exists(newFilePath))
            {
                return newFilePath;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get path của file config chung
        /// </summary>
        /// <param name="directoryFile"></param>
        /// <returns></returns>
        public static string GetPathConfigCommon(string directoryFile)
        {
            string pathCommon = string.Empty;
            bool existsDirectory = false;
            int loopFindCount = 0;
            do
            {
                pathCommon = Path.Combine(directoryFile, "Config", "appsettings.json");
                existsDirectory = File.Exists(pathCommon);
                directoryFile = Path.GetDirectoryName(directoryFile);
                loopFindCount++;
            } while (!(existsDirectory || loopFindCount >= 15));

            if (existsDirectory)
            {
                return pathCommon;
            }
            return string.Empty;
        }
        /// <summary>
        /// Merge file json 2 vào file json 1
        /// </summary>
        /// <param name="jsonFilePath1"></param>
        /// <param name="jsonFilePath2"></param>
        public static void MergeJsonFile(string jsonFilePath1, string jsonFilePath2)
        {
            if (string.IsNullOrEmpty(jsonFilePath1) || string.IsNullOrEmpty(jsonFilePath2))
                return;
            JObject jObject1 = JObject.Parse(File.ReadAllText(jsonFilePath1));
            JObject jObject2 = JObject.Parse(File.ReadAllText(jsonFilePath2));
            JObject jObjectNew = GetObjectMerge(jObject1, jObject2);
            var arrayByte = Encoding.UTF8.GetBytes(jObjectNew.ToString());
            using var fileStream = new FileStream(jsonFilePath1, FileMode.Create, FileAccess.Write);
            fileStream.Write(arrayByte, 0, arrayByte.Length);
            fileStream.Close();
        }
        /// <summary>
        /// merge JObject2 vào JObject1 và trả về JObject1
        /// </summary>
        /// <param name="jObject1"></param>
        /// <param name="jObject2"></param>
        /// <returns></returns>
        public static JObject GetObjectMerge(JObject jObject1, JObject jObject2)
        {
            if (jObject1 != null)
            {
                jObject1.Merge(jObject2, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge });
                return jObject1;
            }
            return jObject2;
        }
    }
}
