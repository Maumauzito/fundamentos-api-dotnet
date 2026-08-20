using System;
using System.Collections.Generic;
using System.Text;

namespace Fiap.GestaoFinanca.Infrastructure.Simulations
{
    public sealed class DataBaseInstabilitySimulator
    {
        private int _attemps;

        public void SimulateFailure()
        { 
            _attemps++;

            if (_attemps <= 2)
            {
                throw new InvalidOperationException("============== Falha transitoria simulada ============");
            }
        
        
        }

    }
}
