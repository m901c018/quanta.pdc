using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using cns.Services.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace cns.Services
{
    public class ApiService
    {

        public ApiService()
        {
            
        }

        /// <summary> 取得API使用者資料
        /// 
        /// </summary>
        /// <param name="DomainWithEmpNumber">使用者工號+域名(ex:quenta\\100F019)</param>
        /// <param name="api_QueryUserInfo">返回資料model</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <returns></returns>
        public static bool GetApiUserInfo(string DomainWithEmpNumber,ref Api_QueryUserInfo api_QueryUserInfo, ref string ErrorMsg)
        {
            Api_postUserInfo api_Postdata = new Api_postUserInfo();
            api_Postdata.EmployeeId = DomainWithEmpNumber;
            api_Postdata.EMail = "";

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        #region 呼叫遠端 Web API
                        string FooUrl = $"http://qcicore01/SAM_API/api/Software/QueryUserInfo";
                        HttpResponseMessage response = null;

                        #region  設定相關網址內容
                        var fooFullUrl = $"{FooUrl}";

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態

                        var fooJSON = JsonConvert.SerializeObject(api_Postdata);
                        using (var fooContent = new StringContent(fooJSON, Encoding.UTF8, "application/json"))
                        {
                            response = client.PostAsync(fooFullUrl, fooContent).Result;
                        }
                        #endregion
                        #endregion

                        #region 處理呼叫完成 Web API 之後的回報結果
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = response.Content.ReadAsStringAsync().Result;
                                api_QueryUserInfo = new Api_QueryUserInfo();
                                string jsonString = strResult.Replace("[", "").Replace("]", "").Replace("\\", "");
                                jsonString = jsonString.TrimStart('\"');
                                jsonString = jsonString.TrimEnd('\"');

                                api_QueryUserInfo = JsonConvert.DeserializeObject<Api_QueryUserInfo>(jsonString);
                            }

                        }
                        else
                        {
                            ErrorMsg = "應用程式呼叫 API 發生異常";
                            return false;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ErrorMsg = "應用程式呼叫 API 發生異常";
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary> 取得API部門名稱
        /// 
        /// </summary>
        /// <param name="COMCOD">公司代碼</param>
        /// <param name="DEPCOD">部門代碼</param>
        /// <param name="api_QueryDept">返回資料model</param>
        /// <param name="ErrorMsg">錯誤資訊</param>
        /// <returns></returns>
        public static bool GetDeptName(string COMCOD,string DEPCOD, ref Api_QueryDept api_QueryDept, ref string ErrorMsg)
        {
            Api_postDept Api_postDept = new Api_postDept();
            Api_postDept.COMCOD = COMCOD;
            Api_postDept.DEPCOD = DEPCOD;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        #region 呼叫遠端 Web API
                        string FooUrl = $"http://qcicore01/SAM_API/api/Software/QueryDeptName";
                        HttpResponseMessage response = null;

                        #region  設定相關網址內容
                        var fooFullUrl = $"{FooUrl}";

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態

                        var fooJSON = JsonConvert.SerializeObject(Api_postDept);
                        using (var fooContent = new StringContent(fooJSON, Encoding.UTF8, "application/json"))
                        {
                            response = client.PostAsync(fooFullUrl, fooContent).Result;
                        }
                        #endregion
                        #endregion

                        #region 處理呼叫完成 Web API 之後的回報結果
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = response.Content.ReadAsStringAsync().Result;
                                api_QueryDept = new Api_QueryDept();
                                string jsonString = strResult.Replace("[", "").Replace("]", "").Replace("\\", "");
                                jsonString = jsonString.TrimStart('\"');
                                jsonString = jsonString.TrimEnd('\"');

                                api_QueryDept = JsonConvert.DeserializeObject<Api_QueryDept>(jsonString);
                            }

                        }
                        else
                        {
                            ErrorMsg = "應用程式呼叫 API 發生異常";
                            return false;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ErrorMsg = "應用程式呼叫 API 發生異常";
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
