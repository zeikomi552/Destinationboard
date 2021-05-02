using Destinationboard.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class StaffInfoM : StaffMasterBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StaffInfoM()
        {
            // 初期化時にtrueをセットする
            this.Display = true;
        }

        /// <summary>
        /// コンストラクタ(StaffMasterReplyを変換する)
        /// </summary>
        /// <param name="reply">StaffMasterReply</param>
        public StaffInfoM(StaffMasterReply reply)
        {
            this.Copy(reply);
        }

        /// <summary>
        /// Copy処理(StaffMasterReplyを変換する)
        /// </summary>
        /// <param name="reply">StaffMasterReply</param>
        public void Copy(StaffMasterReply reply)
        {
            this.StaffID = reply.StaffID;
            this.StaffName = reply.StaffName;
            this.SortOrder = reply.SortOrder;
            this.QRCode = reply.QRCode;
            this.FelicaID = reply.FelicaID;
            this.CreateDate = DateTime.ParseExact( reply.CreateDate, "yyyy/MM/dd HH:mm:ss", null);
            this.CreateUser = reply.CreateUser;
            this.Display = reply.Display;
        }
    }
}
