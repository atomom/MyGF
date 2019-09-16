using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class MyDataTableHelper : DefaultDataTableHelper
    {
        public override IEnumerable<GameFrameworkSegment<string>> GetDataRowSegments(string text)
        {
            List<GameFrameworkSegment<string>> dataRowSegments = new List<GameFrameworkSegment<string>>();
            GameFrameworkSegment<string> dataRowSegment;
            int position = 0;
            int line = 0;
            while ((dataRowSegment = ReadLine(text, ref position)) != default(GameFrameworkSegment<string>))
            {
                line++;
                if (text[dataRowSegment.Offset] == '#' || line <= 4)
                {
                    continue;
                }

                dataRowSegments.Add(dataRowSegment);
            }

            return dataRowSegments;
        }

        private GameFrameworkSegment<string> ReadLine(string text, ref int position)
        {
            int length = text.Length;
            int offset = position;
            while (offset < length)
            {
                char ch = text[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset - position > 0)
                        {
                            GameFrameworkSegment<string> segment = new GameFrameworkSegment<string>(text, position, offset - position);
                            position = offset + 1;
                            if (((ch == '\r') && (position < length)) && (text[position] == '\n'))
                            {
                                position++;
                            }

                            return segment;
                        }

                        offset++;
                        position++;
                        break;

                    default:
                        offset++;
                        break;
                }
            }

            if (offset > position)
            {
                GameFrameworkSegment<string> segment = new GameFrameworkSegment<string>(text, position, offset - position);
                position = offset;
                return segment;
            }

            return default(GameFrameworkSegment<string>);
        }
    }
}