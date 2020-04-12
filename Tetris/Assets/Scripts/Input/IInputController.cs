namespace Application.Controllers
{
    public interface IInputController
    {
        bool Pause { get; }
        bool MoveLeft { get; }
        bool MoveRight { get; }
        bool RotateCounterClockwise { get; }
        bool RotateClockwise { get; }
        bool DropHard { get; }
        bool DropSoft { get; }
        bool HoldPiece { get; }
    }
}