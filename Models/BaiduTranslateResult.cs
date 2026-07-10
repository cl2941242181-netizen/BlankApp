using System;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;

namespace BlankApp.Models
{
    internal class BaiduTranslateResult
    {
        [JsonProperty("from")]
        public string FromLan { get; set; }

        [JsonProperty("to")]
        public string ToLan { get; set; }

        [JsonProperty("trans_result")]
        public List<BaiduTranslationItem> TransResult { get; set; }

        // 如果出错，百度会返回以下两个字段
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty("error_msg")]
        public string ErrorMsg { get; set; }

        // 辅助属性：判断本次请求是否成功
        public bool IsSuccess => string.IsNullOrEmpty(ErrorCode);
    }
    
}
