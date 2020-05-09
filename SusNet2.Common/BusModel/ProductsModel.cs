using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusNet2.Common.BusModel
{
    /// <summary>
    /// pc发送到硬件终端的消息Model
    /// </summary>
    public class ProductsModel
    {
        /// <summary>
        /// 制单号
        /// </summary>
        public string ProcessOrderNo { set; get; }
        /// <summary>
        /// 颜色代码
        /// </summary>
        public string ColorCode { set; get; }
        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessFlowNo { set; get; }
        /// <summary>
        /// 尺码代码
        /// </summary>
        public string SizeCode { set; get; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { set; get; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public int TaskNum { set; get; }
        /// <summary>
        /// 累计完成
        /// </summary>
        public int SucessedTotalNum { set; get; }

        /// <summary>
        /// 今日上线
        /// </summary>
        public int TodayOnlineNum { set; get; }

    }
}
