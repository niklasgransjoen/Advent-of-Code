namespace AOC.Y2019.Day13.P01
{
    public interface IIOPort
    {
        IOReadResult Read();

        /// <summary>
        /// Registers an interpreter to be executed when input becomes available.
        /// </summary>
        void RegisterInterpreterForInput(IInterpreter interpreter);

        void Write(long value);
    }
}