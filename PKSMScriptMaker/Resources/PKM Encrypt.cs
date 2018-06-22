public void encrypt()
        {
            pid = BitConverter.ToUInt32(data, 0);
            checksum = BitConverter.ToUInt16(data, 6);
            uint order = (pid >> (13) & 31) % 24;
            string firstblock = null;
            string secondblock = null;
            string thirdblock = null;
            string fourthblock = null;
            switch (order)
            {
                case 0:
                    firstblock = "A";
                    secondblock = "B";
                    thirdblock = "C";
                    fourthblock = "D";
                    break;
                case 1:
                    firstblock = "A";
                    secondblock = "B";
                    thirdblock = "D";
                    fourthblock = "C";
                    break;
                case 2:
                    firstblock = "A";
                    secondblock = "C";
                    thirdblock = "B";
                    fourthblock = "D";
                    break;
                case 3:
                    firstblock = "A";
                    secondblock = "C";
                    thirdblock = "D";
                    fourthblock = "B";
                    break;
                case 4:
                    firstblock = "A";
                    secondblock = "D";
                    thirdblock = "B";
                    fourthblock = "C";
                    break;
                case 5:
                    firstblock = "A";
                    secondblock = "D";
                    thirdblock = "C";
                    fourthblock = "B";
                    break;
                case 6:
                    firstblock = "B";
                    secondblock = "A";
                    thirdblock = "C";
                    fourthblock = "D";
                    break;
                case 7:
                    firstblock = "B";
                    secondblock = "A";
                    thirdblock = "D";
                    fourthblock = "C";
                    break;
                case 8:
                    firstblock = "B";
                    secondblock = "C";
                    thirdblock = "A";
                    fourthblock = "D";
                    break;
                case 9:
                    firstblock = "B";
                    secondblock = "C";
                    thirdblock = "D";
                    fourthblock = "A";
                    break;
                case 10:
                    firstblock = "B";
                    secondblock = "D";
                    thirdblock = "A";
                    fourthblock = "C";
                    break;
                case 11:
                    firstblock = "B";
                    secondblock = "D";
                    thirdblock = "C";
                    fourthblock = "A";
                    break;
                case 12:
                    firstblock = "C";
                    secondblock = "A";
                    thirdblock = "B";
                    fourthblock = "D";
                    break;
                case 13:
                    firstblock = "C";
                    secondblock = "A";
                    thirdblock = "D";
                    fourthblock = "B";
                    break;
                case 14:
                    firstblock = "C";
                    secondblock = "B";
                    thirdblock = "A";
                    fourthblock = "D";
                    break;
                case 15:
                    firstblock = "C";
                    secondblock = "B";
                    thirdblock = "D";
                    fourthblock = "A";
                    break;
                case 16:
                    firstblock = "C";
                    secondblock = "D";
                    thirdblock = "A";
                    fourthblock = "B";
                    break;
                case 17:
                    firstblock = "C";
                    secondblock = "D";
                    thirdblock = "B";
                    fourthblock = "A";
                    break;
                case 18:
                    firstblock = "D";
                    secondblock = "A";
                    thirdblock = "B";
                    fourthblock = "C";
                    break;
                case 19:
                    firstblock = "D";
                    secondblock = "A";
                    thirdblock = "C";
                    fourthblock = "B";
                    break;
                case 20:
                    firstblock = "D";
                    secondblock = "B";
                    thirdblock = "A";
                    fourthblock = "C";
                    break;
                case 21:
                    firstblock = "D";
                    secondblock = "B";
                    thirdblock = "C";
                    fourthblock = "A";
                    break;
                case 22:
                    firstblock = "D";
                    secondblock = "C";
                    thirdblock = "A";
                    fourthblock = "B";
                    break;
                case 23:
                    firstblock = "D";
                    secondblock = "C";
                    thirdblock = "B";
                    fourthblock = "A";
                    break;
            }
            int z = 0;
            int v = 8;
            //Block A
            UInt16[] blocka = new UInt16[16];
            while (z < 16)
            {
                blocka[z] = BitConverter.ToUInt16(data, v);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            v = 0x28;
            //Block B
            UInt16[] blockb = new UInt16[16];
            while (z < 16)
            {
                blockb[z] = BitConverter.ToUInt16(data, v);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            //Block C
            v = 0x48;
            UInt16[] blockc = new UInt16[16];
            while (z < 16)
            {
                blockc[z] = BitConverter.ToUInt16(data, v);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            //Block D
            UInt16[] blockd = new UInt16[16];
            v = 0x68;
            while (z < 16)
            {
                blockd[z] = BitConverter.ToUInt16(data, v);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            UInt16[] partyb = new UInt16[50];
            //if (partypkm == true)
            //{
            v = 136;
            z = 0;
            while (z < 50)
            {
                partyb[z] = BitConverter.ToUInt16(data, v);
                z = z + 1;
                v = v + 2;
            }
            //}
            z = 0;
            srand(checksum);
            UInt16[] byter = new UInt16[29];
            z = 0;
            v = 8;
            switch (firstblock)
            {
                case "A":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blocka[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "B":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockb[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "C":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockc[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "D":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockd[z] ^ rand());
                        z = z + 1;
                    }
                    break;
            }
            z = 0;
            v = 8;
            while (z < 16)
            {
                data[v] = (byte)(byter[z] & 255);
                data[v + 1] = (byte)(byter[z] >> 8);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            v = 0x28;
            switch (secondblock)
            {
                case "A":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blocka[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "B":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockb[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "C":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockc[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "D":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockd[z] ^ rand());
                        z = z + 1;
                    }
                    break;
            }
            z = 0;
            while (z < 16)
            {
                data[v] = (byte)(byter[z] & 255);
                data[v + 1] = (byte)(byter[z] >> 8);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            v = 0x48;
            switch (thirdblock)
            {
                case "A":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blocka[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "B":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockb[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "C":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockc[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "D":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockd[z] ^ rand());
                        z = z + 1;
                    }
                    break;
            }
            z = 0;
            while (z < 16)
            {
                data[v] = (byte)(byter[z] & 255);
                data[v + 1] = (byte)(byter[z] >> 8);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            v = 0x68;
            switch (fourthblock)
            {
                case "A":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blocka[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "B":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockb[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "C":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockc[z] ^ rand());
                        z = z + 1;
                    }
                    break;
                case "D":
                    while (z < 16)
                    {
                        byter[z] = (ushort)(blockd[z] ^ rand());
                        z = z + 1;
                    }
                    break;
            }
            z = 0;
            while (z < 16)
            {
                data[v] = (byte)(byter[z] & 255);
                data[v + 1] = (byte)(byter[z] >> 8);
                z = z + 1;
                v = v + 2;
            }
            z = 0;
            //Party
            v = 136;
            //if (partypkm == true)
            //{
            srand(pid);
            z = 0;
            UInt16[] partybytes = new UInt16[50];
            while (z < 50)
            {
                partybytes[z] = (ushort)((partyb[z]) ^ (rand()));
                z = z + 1;
            }
            z = 0;
            v = 136;
            while (z < 50)
            {
                data[v] = (byte)(partybytes[z] & 255);
                data[v + 1] = (byte)(partybytes[z] >> 8);
                z = z + 1;
                v = v + 2;
            }
            //}
        }