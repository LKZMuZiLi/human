namespace LKZ.Commands.Voice
{
    /// <summary>
    /// 语音识别成功命令
    /// </summary>
    public struct  VoiceRecognitionResultCommand
    {
        public string text;

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplete;
    }
}
