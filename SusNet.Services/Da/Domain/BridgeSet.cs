using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;

namespace SusNet.Services.Da.Domain
{

    
    /// <summary>
    /// 桥接配置
    /// </summary>
    [Serializable]
    public partial class BridgeSet  {
        public virtual string Id { get; set; }
        /// <summary>
        /// 顺序编号
        /// </summary>
        [Description("顺序编号")]
        public virtual short? BIndex { get; set; }
        /// <summary>
        /// A线主轨
        /// </summary>
        [Description("A线主轨")]
        public virtual short? AMainTrackNumber { get; set; }
        /// <summary>
        /// 制单号
        /// </summary>
        [Description("制单号")]
        public virtual short? ASiteNo { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        [Description("方向")]
        public virtual short? Direction { get; set; }
        /// <summary>
        /// 方向描述
        /// </summary>
        [Description("方向描述")]
        public virtual string DirectionTxt { get; set; }
        /// <summary>
        /// B线主轨
        /// </summary>
        [Description("B线主轨")]
        public virtual short? BMainTrackNumber { get; set; }
        /// <summary>
        /// B线主轨站号
        /// </summary>
        [Description("B线主轨站号")]
        public virtual short? BSiteNo { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Description("是否启用")]
        public virtual bool? Enabled { get; set; }
    }
}
