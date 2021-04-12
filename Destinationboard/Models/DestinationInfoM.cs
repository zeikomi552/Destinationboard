using Destinationboard.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Models
{
    public class DestinationInfoM : DestinationMasterBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DestinationInfoM()
        {
            Guid guidValue = Guid.NewGuid();
            this.DestinationID = guidValue.ToString();

            this.CreateDate = DateTime.Now;
            this.CreateUser = Environment.UserName;
            this.UpdateDate = DateTime.Now;
            this.UpdateUser = Environment.UserName;
        }

        /// <summary>
        /// リプライをコピーするコンストラクタ
        /// </summary>
        public DestinationInfoM(DestinationMasterReply reply)
        {
            this.ActionID = reply.ActionID;
            this.DestinationID = reply.DestinationID;
            this.DestinationName = reply.DestinationName;
            this.CreateDate = DateTime.ParseExact(reply.CreateDate, "yyyy/MM/dd", null);
            this.CreateUser = reply.CreateUser;
            this.UpdateDate = DateTime.ParseExact(reply.UpdateDate, "yyyy/MM/dd", null);
            this.UpdateUser = reply.UpdateUser;
        }

        /// <summary>
        /// リプライをコピーする処理
        /// </summary>
        /// <param name="reply">リプライ</param>
        /// <returns>行先情報</returns>
        public DestinationInfoM Copy(DestinationMasterReply reply)
        {
            return new DestinationInfoM(reply);
        }
    }
}
