using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenyangZC.VOBC
{
    class ObstacleInfo
    {
        Byte NC_Obstacle_;
        public Byte NC_Obstacle
        {
            get { return NC_Obstacle_; }
            set { NC_Obstacle_ = value; }
        }

        UInt16 NID_Obstacle_;
        public UInt16 NID_Obstacle
        {
            get { return NID_Obstacle_; }
            set { NID_Obstacle_ = value; }
        }

        Byte Q_Obstacle_Now_;
        public Byte Q_Obstacle_Now
        {
            get { return Q_Obstacle_Now_; }
            set { Q_Obstacle_Now_ = value; }
        }

        Byte Q_Obstacle_CI_;
        public Byte Q_Obstacle_CI
        {
            get { return Q_Obstacle_CI_; }
            set { Q_Obstacle_CI_ = value; }
        }
    }
}
