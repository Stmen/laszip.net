﻿//===============================================================================
//
//  FILE:  laswriteitemraw_byte.cs
//
//  CONTENTS:
//
//    Implementation of LASwriteItemRaw for BYTE items.
//
//  PROGRAMMERS:
//
//    martin.isenburg@rapidlasso.com  -  http://rapidlasso.com
//
//  COPYRIGHT:
//
//    (c) 2007-2017, martin isenburg, rapidlasso - tools to catch reality
//    (c) of the C# port 2014-2018 by Shinta <shintadono@googlemail.com>
//
//    This is free software; you can redistribute and/or modify it under the
//    terms of the GNU Lesser General Licence as published by the Free Software
//    Foundation. See the COPYING file for more information.
//
//    This software is distributed WITHOUT ANY WARRANTY and without even the
//    implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//  CHANGE HISTORY: omitted for easier Copy&Paste (pls see the original)
//
//===============================================================================

namespace LASzip.Net
{
	class LASwriteItemRaw_BYTE : LASwriteItemRaw
	{
		public LASwriteItemRaw_BYTE(uint number) { this.number = number; }

		public override bool write(laszip.point item, ref uint context)
		{
			try
			{
				outstream.Write(item.extra_bytes, 0, (int)number);
			}
			catch
			{
				return false;
			}

			return true;
		}

		uint number = 0;
	}
}
