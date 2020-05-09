using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SusNet.Services.Utils
{
    public class EnumUtils<T> where T:struct
    {
        /// <summary>  
        /// 用于缓存枚举值的属性值  
        /// </summary>  
        private static readonly Dictionary<object, T> enumAttr = new Dictionary<object, T>();

        ///// <summary>  
        ///// 获取枚举值的名称，该名称由EnumAttribute定义  
        ///// </summary>  
        ///// <param name="value">枚举值</param>  
        ///// <returns>枚举值对应的名称</returns>  
        //public static string GetName(Enum value)
        //{
        //    T ea = GetAttribute(value);
        //    return ea != null ? ea.Name : "";
        //}

        ///// <summary>  
        ///// 获取枚举值的名称，该名称由EnumAttribute定义  
        ///// </summary>  
        ///// <param name="value">枚举值</param>  
        ///// <returns>枚举值对应的名称</returns>  
        //public static string GetDescription(Enum value)
        //{
        //    T ea = GetAttribute(value);
        //    return ea != null ? ea.Description : "";
        //}

        /// <summary>  
        /// 从字符串转换为枚举类型  
        /// </summary>  
        /// <typeparam name="T">枚举类型</typeparam>  
        /// <param name="str">要转为枚举的字符串</param>  
        /// <returns>转换结果</returns>  
        public static T GetEnum<T>(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ApplicationException("排序对象解析出错！");

            try
            {
                T pe = (T)Enum.Parse(typeof(T), str);
                return pe;
            }
            catch
            {
                Type type = typeof(T);
                string templete = "枚举类型{0}中没有定义{1}项！";
                string msg = string.Format(templete, type.ToString(), str);

                throw new ApplicationException("排序对象解析出错！");
            }
        }

        /// <summary>  
        /// 获取枚举值定义的属性  
        /// </summary>  
        /// <param name="value">枚举对象</param>  
        /// <returns>获取枚举对象的描述属性值</returns>  
        public static T GetAttribute(Enum value)
        {
            if (enumAttr.ContainsKey(value))
            {
                T ea = enumAttr[value];
                return ea;
            }
            else
            {
                FieldInfo field = value.GetType().GetField(value.ToString());
                if (field == null) return default(T);
                T ea = default(T);
                object[] attributes = field.GetCustomAttributes(typeof(T), true);
                if (attributes != null && attributes.Length > 0)
                {
                    ea = (T)attributes[0];
                }
                enumAttr[value] = ea;
                return ea;
            }
        }
    }
}
