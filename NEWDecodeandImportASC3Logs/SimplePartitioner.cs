using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace NEWDecodeandImportASC3Logs
{
    // Represent a chunk partitioner
    public class SimplePartitioner<T> : OrderablePartitioner<T>
    {
        private readonly IEnumerator<T> m_input;

        public SimplePartitioner(IEnumerable<T> input)
            : base(true, false, true)
        {
            m_input = input.GetEnumerator();
        }

        // Must override to return true.
        public override bool SupportsDynamicPartitions
        {
            get
            {
                return true;
            }
        }

        public override IList<IEnumerator<KeyValuePair<long, T>>>
            GetOrderablePartitions(int partitionCount)
        {
            var dynamicPartitions = GetOrderableDynamicPartitions();
            var partitions =
                new IEnumerator<KeyValuePair<long, T>>[partitionCount];

            for (int i = 0; i < partitionCount; i++)
            {
                partitions[i] = dynamicPartitions.GetEnumerator();
            }
            return partitions;
        }

        public override IEnumerable<KeyValuePair<long, T>>
            GetOrderableDynamicPartitions()
        {
            return new ReaderDynamicPartitions(m_input);
        }

        private class ReaderDynamicPartitions
            : IEnumerable<KeyValuePair<long, T>>
        {
            private object syncObject = new object();
            private bool finished = false;
            private IEnumerator<T> m_input;
            private int m_pos = 0;

            internal ReaderDynamicPartitions(IEnumerator<T> input)
            {
                m_input = input;
            }

            public IEnumerator<KeyValuePair<long, T>> GetEnumerator()
            {
                while (true)
                {
                    var toReturn = new KeyValuePair<long, T>();

                    lock (syncObject)
                    {
                        if (!finished && !m_input.MoveNext())
                        { finished = true; }
                        if (!finished)
                        {
                            toReturn = new KeyValuePair<long, T>(m_pos, m_input.Current);
                        }
                    }
                    if (finished)
                        yield break;
                    else
                        yield return toReturn;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

        }
    }
}
