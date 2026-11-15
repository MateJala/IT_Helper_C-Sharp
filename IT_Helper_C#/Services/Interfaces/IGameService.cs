namespace mate.Interface;

public interface IGameService
{
    int Width { get; }
    int Height { get; }
    bool IsRunning { get; set; }

    void Background();
    void DrawMenu(string[] menuArray, int selectedIndex);
    void ProgressBar(int time = 1500);
    int SelectMenu(string title, string[] options);
}