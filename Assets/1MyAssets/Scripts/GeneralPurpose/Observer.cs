using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface NewSegmentObserver
{
    void ObserveNewSegment(Segment segment);
}
public interface RemovedSegmentObserver
{
    void ObserveRemovedSegment(Segment segment);
}
//public interface NetworkChangeObserver
//{
//    void ObserveChange();
//}
public interface NetworkObserver : NewSegmentObserver, RemovedSegmentObserver { }