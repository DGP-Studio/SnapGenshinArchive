using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Snap.Data.Json
{
    /// <summary>
    /// Json操作
    /// </summary>
    public static partial class Json
    {
        /// <summary>
        /// 将JSON反序列化为指定的.NET类型
        /// </summary>
        /// <typeparam name="T">要反序列化的对象的类型</typeparam>
        /// <param name="value">要反序列化的JSON</param>
        /// <returns>Json字符串中的反序列化对象, 如果反序列化失败会抛出异常</returns>
        public static T? ToObject<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// 将JSON反序列化为指定的.NET类型
        /// 若为null则返回一个新建的实例
        /// </summary>
        /// <typeparam name="T">指定的类型</typeparam>
        /// <param name="value">字符串</param>
        /// <returns>Json字符串中的反序列化对象, 如果反序列化失败会抛出异常</returns>
        public static T ToObjectOrNew<T>(string value)
            where T : new()
        {
            return ToObject<T>(value) ?? new T();
        }

        /// <summary>
        /// 将指定的对象序列化为JSON字符串
        /// </summary>
        /// <param name="value">要序列化的对象</param>
        /// <param name="indented">是否缩进</param>
        /// <returns>对象的JSON字符串表示形式</returns>
        public static string Stringify(object? value, bool indented = true)
        {
            JsonSerializerSettings jsonSerializerSettings = new()
            {
                DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.FFFFFFFK",
                Formatting = indented ? Formatting.Indented : Formatting.None,
            };
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        /// <summary>
        /// 使用 <see cref="FileMode.Open"/>, <see cref="FileAccess.Read"/> 和 <see cref="FileShare.Read"/> 从文件中读取后转化为实体类
        /// </summary>
        /// <typeparam name="T">要反序列化的对象的类型</typeparam>
        /// <param name="fileName">存放JSON数据的文件路径</param>
        /// <returns>JSON字符串中的反序列化对象, 如果反序列化失败则抛出异常，若文件不存在则返回 <see langword="null"/></returns>
        public static T? FromFile<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                // FileShare.Read is important to read some log file
                using (StreamReader sr = new(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    return ToObject<T>(sr.ReadToEnd());
                }
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 使用 <see cref="FileMode.Open"/>, <see cref="FileAccess.Read"/> 和 <see cref="FileShare.Read"/> 从文件中读取后转化为实体类
        /// 若为null则返回一个新建的实例
        /// </summary>
        /// <typeparam name="T">要反序列化的对象的类型</typeparam>
        /// <param name="fileName">存放JSON数据的文件路径</param>
        /// <returns>JSON字符串中的反序列化对象</returns>
        public static T FromFileOrNew<T>(string fileName)
            where T : new()
        {
            return FromFile<T>(fileName) ?? new T();
        }

        /// <summary>
        /// 从文件中读取后转化为实体类
        /// </summary>
        /// <typeparam name="T">要反序列化的对象的类型</typeparam>
        /// <param name="file">存放JSON数据的文件</param>
        /// <returns>JSON字符串中的反序列化对象</returns>
        public static T? FromFile<T>(FileInfo file)
        {
            using (StreamReader sr = file.OpenText())
            {
                return ToObject<T>(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// 将对象保存到文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="value">对象</param>
        public static void ToFile(string fileName, object? value)
        {
            File.WriteAllText(fileName, Stringify(value));
        }
    }
}