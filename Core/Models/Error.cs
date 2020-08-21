using Core.Models;

namespace Core.Service
{
    public struct Error
    {
        public int Cod { get; set; }
        public string Message {
            get => Message;
            set { Message = value.FirstCharUpper(); } }
    }
}
