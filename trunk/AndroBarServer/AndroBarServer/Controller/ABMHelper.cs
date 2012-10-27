﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AndroBarServer.Model;
using System.Collections;

namespace AndroBarServer.Controller
{
    public static class ABMHelper
    {
        public static category GetCategory(int id)
        {
            return GetCategory(null, id);
        }

        public static category GetCategory(androbarEntities db, int id)
        {
            if (db == null) db = new androbarEntities();
            try
            {
                return (from categories in db.categories
                        where categories.Id == id
                        select categories).First();
            }
            catch
            {
                return null;
            }
        }

        public static product GetProduct(int id)
        {
            return GetProduct(null, id);
        }

        public static product GetProduct(androbarEntities db, int id)
        {
            if (db == null) db = new androbarEntities();
            try
            {
                return (from products in db.products
                        where products.Id == id
                        select products).First();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] StreamToByteArray(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
    }
}