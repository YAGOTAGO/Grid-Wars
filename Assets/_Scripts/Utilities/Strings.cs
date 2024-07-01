using Unity.Netcode;

public struct Strings : INetworkSerializable
{
    string[] myStrings;

    public Strings(string[] myStrings)
    {
        this.myStrings = myStrings;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsWriter)
        {
            FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
            fastBufferWriter.WriteValueSafe(myStrings.Length);

            for (int i = 0; i < myStrings.Length; i++)
            {
                fastBufferWriter.WriteValueSafe(myStrings[i]);
            }
        }

        if (serializer.IsReader)
        {
            FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
            fastBufferReader.ReadValueSafe(out int length);

            myStrings = new string[length];

            for (int i = 0; i < length; i++)
            {
                fastBufferReader.ReadValueSafe(out myStrings[i]);
            }
        }
    }

    public string[] MyStrings { get => myStrings; set => myStrings = value; }
}