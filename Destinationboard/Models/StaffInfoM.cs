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
            this.CreateDate = DateTime.ParseExact(reply.CreateDate, "yyyy/MM/dd HH:mm:ss", null);
            this.CreateUser = reply.CreateUser;
            this.Display = reply.Display;
        }

        /// <summary>
        /// サンプルのQRコード
        /// </summary>
        public string SampleQRCode
        {
            get
            {
                if (string.IsNullOrEmpty(this.QRCode))
                {
                    char rs = (char)0x1E;
                    char gs = (char)0x1D;
                    char eot = (char)0x04D;
                    StringBuilder qrcode = new StringBuilder();
                    qrcode.Append("[)>20");
                    qrcode.Append(rs);
                    qrcode.Append("1HS" + this.StaffID);
                    qrcode.Append(gs);
                    qrcode.Append("1HN" + this.StaffName);
                    qrcode.Append(rs);
                    qrcode.Append(eot);
                    return qrcode.ToString();
                }
                else
                {
                    return this.QRCode;
                }
            }
        }
    
    }
}
