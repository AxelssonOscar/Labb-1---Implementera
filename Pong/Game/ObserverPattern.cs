using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongLecture.Game
{
    public interface IObserver
    {
        void Pause(bool pause);
    }
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Pause(bool pause);
    }

}
