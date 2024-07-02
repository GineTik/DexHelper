namespace Backend.Infrastructure.Implementation.TokensApi.Types;

public class NewTokensPayload
{
    #region classes

    public class InstructionItem
    {
        public Block Block { get; set; } = default!;
        public InstructionContent Instruction { get; set; } = default!;
        public Transaction Transaction { get; set; } = default!;
    }
     
    public class Block
    {
        public DateTime Time { get; set; } = default!;
    }
    
    public class InstructionContent
    {
        public List<Account> Accounts { get; set; } = default!;
        public Program Program { get; set; } = default!;
    }
    
    public class Account
    {
        public string Address { get; set; } = default!;
    }
    
    public class Program
    {
        public string Address { get; set; } = default!;
        public List<Argument> Arguments { get; set; } = default!;
        public string Method { get; set; } = default!;
    }
    
    public class Argument
    {
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public Value Value { get; set; } = default!;
    }
    
    public class Value
    {
        public int Integer { get; set; } = default!;
        public string String { get; set; } = default!;
    }
    
    public class Transaction
    {
        public string Signature { get; set; } = default!;
    }
    
    public class SolanaContent
    {
        public List<InstructionItem> Instructions { get; set; } = default!;
    }

    #endregion
    
    public SolanaContent Solana { get; set; } = default!;
}