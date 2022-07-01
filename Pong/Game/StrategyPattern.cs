using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongLecture.Game
{
    //Paddlarnas storlek bestäms under runtime med hjälp av StrategyPattern. 
    public interface IPaddleSkill
    {
        float GetHeight(float height);
    }

    public class EasyPaddle : IPaddleSkill
    {
        public float GetHeight(float height) => height * 1.5f;
    }

    public class MediumPaddle : IPaddleSkill
    {
        public float GetHeight(float height) => height;
    }

    public class HardPaddle : IPaddleSkill
    {
        public float GetHeight(float height) => height * 0.5f;
    }

}
