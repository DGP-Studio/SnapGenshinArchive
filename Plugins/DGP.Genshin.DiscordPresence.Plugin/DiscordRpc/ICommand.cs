internal interface ICommand
{
    IPayload PreparePayload(long nonce);
}