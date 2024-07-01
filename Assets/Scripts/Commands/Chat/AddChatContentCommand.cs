using LKZ.Enum;
using System;

namespace LKZ.Chat.Commands
{
    /// <summary>
    /// 添加一个聊天命令
    /// </summary>
    public struct AddChatContentCommand
    {
        public InfoType infoType;

        /// <summary>
        /// 添加文本的委托
        /// 这个参数是那个显示的数据添加数据用的
        /// </summary>
        public Action<Action<string>> _addTextAction;


    }
}
