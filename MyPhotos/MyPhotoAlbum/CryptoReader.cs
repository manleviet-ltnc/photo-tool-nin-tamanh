using System;
using System.IO;
using System.Text;


namespace Manning.MyPhotoAlbum
{
    class CryptoReader: StreamReader
    {
        private CryptoTextBase _base;
        private CryptoTextBase CryptoBase
        {
            get { return _base; }
        }
        public CryptoReader(string path, string password) : base(path)
        {
            if (path == null || path.Length == 0)
                throw new ArgumentNullException("path");
            if (password == null || password.Length == 0)
                throw new ArgumentNullException("password");

            _base = new CryptoTextBase(password);
        }
        public override string ReadLine()
        {
            string encryted = base.ReadLine();
            if (encryted == null || encryted.Length == 0)
                return encryted;
            else
                return CryptoBase.ProcessText(encryted, false);
        }
        public string ReadUnencryptedLine()
        {
            return base.ReadLine();
                
        }

    }

}
