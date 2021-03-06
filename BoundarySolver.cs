﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barnacle
{
    class BoundarySolver : Solver
    {
        public new List<BoundarySolverResult> resultRepository;
        public Zone zone;


        public BoundarySolver(Metric metric, BoundarySolverResult solverResult) : base(metric, solverResult)
        {
            resultRepository = new List<BoundarySolverResult>();
        }

        public BoundarySolver WithZone(Zone newZone)
        {
            this.zone = newZone;
            return this;
        }

        public new List<BoundarySolverResult> Solve()
        {
            
            BoundarySolverResult branch = new BoundarySolverResult();
            Grow(branch, 0);

            foreach (BoundarySolverResult cur in resultRepository)
            {
                int curValue = cur.CalculateTotalStall();
            }
            // double max = resultRepository.ElementAt(i).CalculateTotalStall();
            // MessageBox.Show(resultRepository.Count().ToString());
            resultRepository.Sort((a, b) => (b.totalCount - a.totalCount));

            return this.resultRepository;
        }

        void Grow(BoundarySolverResult branch, int baseLineID)
        {
            if (baseLineID >= this.zone.edges.Length)
            {
                // Change branch
                branch.ChangeToInnerLine();
                resultRepository.Add(branch);


                return;
            }

            foreach (CarStallMeta meta in CarStallMeta.SINGLE_META_LIST)
            {
                RowNode newNode = new CarStallRow(
                    baseLineID,
                     zone.OffsetInZone(zone.edges[baseLineID], baseLineID, 0),
                     meta,
                     zone);
                branch.Add(newNode);
                Grow(branch.Clone(), baseLineID + 1);
                branch.RemoveLast();
            }

        }

        public BoundarySolverResult GetBest(int n)
        {
            if (n >= resultRepository.Count())
            {
                return null;
            }

           
            // double max = resultRepository.ElementAt(i).CalculateTotalStall();
            // MessageBox.Show(resultRepository.Count().ToString());
            BoundarySolverResult res = resultRepository[n];
            //writeLog(res.endNode);
            return res;
        }

    }
}
