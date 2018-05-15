using System;
using System.Web;

namespace DFramework.Infrastructure
{
    public class CookieHelper
    {
        #region 获取Cookie

        public static string GetCookieValue(string cookieName)
        {
            return GetCookieValue(cookieName, null);
        }

        public static string GetCookieValue(string cookieName, string key)
        {
            var request = HttpContext.Current.Request;
            if (request != null)
            {
                return GetCookieValue(request.Cookies[cookieName], key);
            }
            return "";
        }

        public static string GetCookieValue(HttpCookie cookie, string key)
        {
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                {
                    return cookie.Values[key];
                }
                return cookie.Value;
            }
            return "";
        }

        public static HttpCookie GetCookie(string cookieName)
        {
            var request = HttpContext.Current.Request;
            if (request != null)
            {
                return request.Cookies[cookieName];
            }
            return null;
        }

        #endregion 获取Cookie

        #region 删除Cookie

        public static void RemoveCookie(string cookieName)
        {
            RemoveCookie(cookieName, null);
        }

        public static void RemoveCookie(string cookieName, string key)
        {
            var response = HttpContext.Current.Response;
            if (response != null)
            {
                var cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                    {
                        cookie.Values.Remove(key);
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        response.Cookies.Add(cookie);
                    }
                }
            }
        }

        #endregion 删除Cookie

        #region 设置/修改Cookie

        public static void SetCookie(string cookieName, string key, string value)
        {
            SetCookie(cookieName, key, value, null);
        }

        public static void SetCookie(string key, string value)
        {
            SetCookie(key, null, value, null);
        }

        public static void SetCookie(string key, string value, DateTime expires)
        {
            SetCookie(key, null, value, expires);
        }

        public static void SetCookie(string cookieName, DateTime expires)
        {
            SetCookie(cookieName, null, null, expires);
        }

        public static void SetCookie(string cookieName,
            string key,
            string value,
            DateTime? expires)
        {
            var response = HttpContext.Current.Response;
            if (response != null)
            {
                var cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                    {
                        cookie.Values.Add(key, value);
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        cookie.Value = value;
                    }
                    if (expires != null)
                    {
                        cookie.Expires = expires.Value;
                    }
                    response.SetCookie(cookie);
                }
            }
        }

        #endregion 设置/修改Cookie

        #region 添加Cookie

        public static void AddCookie(string key,
            string value,
            string domain = null)
        {
            var cookie = new HttpCookie(key, value);
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            AddCookie(cookie);
        }

        public static void AddCookie(string key,
            string value,
            DateTime expires,
            string domain = null)
        {
            var cookie = new HttpCookie(key, value)
            {
                Expires = expires
            };
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            AddCookie(cookie);
        }

        public static void AddCookie(string cookieName,
            string key,
            string value,
            string domain = null)
        {
            var cookie = new HttpCookie(cookieName);
            cookie.Values.Add(key, value);
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            AddCookie(cookie);
        }

        public static void AddCookie(string cookieName,
            DateTime expires,
            string domain = null)
        {
            var cookie = new HttpCookie(cookieName)
            {
                Expires = expires
            };
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            AddCookie(cookie);
        }

        /// <summary>
        /// 添加为Cookie,Values集合
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        /// <param name="domain"></param>
        public static void AddCookie(string cookieName,
            string key,
            string value,
            DateTime expires,
            string domain = null)
        {
            var cookie = new HttpCookie(cookieName)
            {
                Expires = expires,
            };
            cookie.Values.Add(key, value);
            if (!string.IsNullOrWhiteSpace(domain))
            {
                cookie.Domain = domain;
            }
            AddCookie(cookie);
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void AddCookie(HttpCookie cookie)
        {
            var response = HttpContext.Current.Response;
            if (response != null)
            {
                //
                // 摘要:
                //     获取或设置一个值，该值指定 Cookie 是否可通过客户端脚本访问。
                //
                // 返回结果:
                //     如果 Cookie 具有 HttpOnly 特性且不能通过客户端脚本访问，则为 true；否则为 false。默认值为 false。
                cookie.HttpOnly = true;
                //
                // 摘要:
                //     获取或设置要与当前 Cookie 一起传输的虚拟路径。
                //
                // 返回结果:
                //     要与此 Cookie 一起传输的虚拟路径。默认为 /，也就是服务器根目录。
                cookie.Path = "/";
                response.AppendCookie(cookie);
            }
        }

        #endregion 添加Cookie
    }
}