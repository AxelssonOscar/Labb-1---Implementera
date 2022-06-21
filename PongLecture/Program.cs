using Raylib_cs;

namespace PongLecture
{


    /*
     * Här är ett exempel på det klassiska spelet Pong. För grafik används en wrapper för framworket Raylib.
     * W/S styr vänster paddel och Upp/Ner-pil styr höger paddel. Mellanslag för paus och Enter för att börja om.
     * Escape eller krysset för att stänga spelet.
     * 
     * I detta program så har jag implementerat följande designmönster: Singleton, Observer samt Strategy.
     * 
     * Singleton för klassen Pong då endast ett objekt av typen Pong ska existera.
     * Observer används på så sätt att spelobjekten är observers och själva spelet Pong är subject.
     * Strategy används för att bestämma svårigheten för spelarnas paddlar. 
     */
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(800, 480, "Pong");
            Raylib.SetWindowState(ConfigFlags.FLAG_VSYNC_HINT);

            Game.Pong pong = Game.Pong.Instance();
            pong.GameLoop();

            Raylib.CloseWindow();
        }
    }

}




