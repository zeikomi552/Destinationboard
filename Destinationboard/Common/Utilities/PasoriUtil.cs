using PCSC;
using PCSC.Iso7816;
using PCSC.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common.Utilities
{
    public class PasoriUtil
    {
        ISCardMonitor _Monitor = MonitorFactory.Instance.Create(SCardScope.System);

        public ISCardMonitor Monitor
        {
            get
            {
                return _Monitor;
            }
        }

        /// <summary>
        /// Readerの名前リスト
        /// </summary>
        public List<string> ReaderNames { get; set; }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PasoriUtil()
        {
            // 接続しているReaderのリスト取得
            var tmp = GetReaderNames();

            // 1つ以上取得できた場合
            if (tmp != null && tmp.Length > 0)
            {
                // 保持
                this.ReaderNames = GetReaderNames().ToList<string>();
            }
            else
            {
                // nullのセット
                this.ReaderNames = null;
            }
        }
        #endregion

        #region Readerの名前を取得
        /// <summary>
        /// Readerの名前を取得
        /// </summary>
        /// <returns></returns>
        public static string[] GetReaderNames()
        {
            // インスタンスの確率
            using (var context = ContextFactory.Instance.Establish(SCardScope.System))
            {
                // リーダーの取得
                return context.GetReaders();
            }
        }
        #endregion

        #region コレクションの数を確認する
        /// <summary>
        /// コレクションの数を確認する
        /// </summary>
        /// <param name="readerNames">Readerの名前リスト</param>
        /// <returns></returns>
        public static bool IsEmpty(ICollection<string> readerNames) => readerNames == null || readerNames.Count < 1;
        #endregion

        #region UID(IDm)を取得する 
        /// <summary>
        /// UID(IDm)を取得する 
        /// </summary>
        /// <param name="readername">Readerの名称</param>
        /// <returns>UID(IDm)</returns>
        public static string GetUID(string readername)
        {
            using (var context = ContextFactory.Instance.Establish(SCardScope.System))
            {
                using (var rfidReader = context.ConnectReader(readername, SCardShareMode.Shared, SCardProtocol.Any))
                {
                    var apdu = new CommandApdu(IsoCase.Case2Short, rfidReader.Protocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetData,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0 // We don't know the ID tag size
                    };

                    using (rfidReader.Transaction(SCardReaderDisposition.Leave))
                    {
                        Console.WriteLine("Retrieving the UID .... ");

                        var sendPci = SCardPCI.GetPci(rfidReader.Protocol);
                        var receivePci = new SCardPCI(); // IO returned protocol control information.

                        var receiveBuffer = new byte[256];
                        var command = apdu.ToArray();

                        var bytesReceived = rfidReader.Transmit(
                            sendPci, // Protocol Control Information (T0, T1 or Raw)
                            command, // command APDU
                            command.Length,
                            receivePci, // returning Protocol Control Information
                            receiveBuffer,
                            receiveBuffer.Length); // data buffer

                        var responseApdu =
                            new ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol);


                        return responseApdu.HasData ? BitConverter.ToString(responseApdu.GetData()) : string.Empty;
                    }
                }
            }
        }
        #endregion

        #region パソリのモニタリングスタート
        /// <summary>
        /// パソリのモニタリングスタート
        /// </summary>
        public void MonitorStart()
        {
            if (!PasoriUtil.IsEmpty(this.ReaderNames))
            {
                _Monitor.Start(this.ReaderNames.First());
            }
        }
        #endregion

        #region Pasoriのモニタリングストップ
        /// <summary>
        /// Pasoriのモニタリングストップ
        /// </summary>
        public void MonitorStop()
        {

            if (!PasoriUtil.IsEmpty(this.ReaderNames))
            {
                _Monitor.Cancel();
            }
        }
        #endregion
    }
}
