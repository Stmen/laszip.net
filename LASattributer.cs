﻿//===============================================================================
//
//  FILE:  lasattributer.cs
//
//  CONTENTS:
//
//    This class assists with handling the "extra bytes" that allow storing
//    additional per point attributes.
//
//  PROGRAMMERS:
//
//    martin.isenburg@rapidlasso.com  -  http://rapidlasso.com
//
//  COPYRIGHT:
//
//    (c) 2005-2017, martin isenburg, rapidlasso - fast tools to catch reality
//    (c) of the C# port 2017-2017 by Shinta <shintadono@googlemail.com>
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

using System.Collections.Generic;

namespace LASzip.Net
{
	public class LASattributer
	{
		public int number_attributes;
		public List<LASattribute> attributes;
		public List<int> attribute_starts;
		public List<int> attribute_sizes;

		public void clean_attributes()
		{
			number_attributes = 0;
			attributes = null;
			attribute_starts = null;
			attribute_sizes = null;
		}

		public bool init_attributes(IEnumerable<LASattribute> attributes)
		{
			clean_attributes();
			try
			{
				this.attributes = new List<LASattribute>();
				attribute_starts = new List<int>();
				attribute_sizes = new List<int>();
			}
			catch
			{
				return false;
			}

			int start = 0;
			foreach (var attribute in attributes)
			{
				number_attributes++;
				this.attributes.Add(new LASattribute(attribute));
				attribute_starts.Add(start);
				attribute_sizes.Add(attribute.get_size());
				start += attribute.get_size();
			}

			return true;
		}

		public int add_attribute(LASattribute attribute)
		{
			if (attribute.get_size() == 0) return -1;

			try
			{
				int start = 0;

				if (attributes == null)
				{
					number_attributes = 0;
					attributes = new List<LASattribute>(1);
					attribute_starts = new List<int>(1);
					attribute_sizes = new List<int>(1);
				}
				else start = attribute_starts[number_attributes - 1] + attribute_sizes[number_attributes - 1];

				number_attributes++;
				attributes.Add(new LASattribute(attribute));
				attribute_starts.Add(start);
				attribute_sizes.Add(attribute.get_size());

				return number_attributes - 1;
			}
			catch
			{
				return -1;
			}
		}

		public short get_attributes_size()
		{
			return (short)(attributes != null ? attribute_starts[number_attributes - 1] + attribute_sizes[number_attributes - 1] : 0);
		}

		public int get_attribute_index(string name)
		{
			if (name.Length > 31) name = name.Substring(0, 31);

			for (int i = 0; i < number_attributes; i++)
			{
				if (attributes[i].name == name) return i;
			}
			return -1;
		}

		public int get_attribute_start(int index)
		{
			return index >= 0 && index < number_attributes ? attribute_starts[index] : -1;
		}

		public int get_attribute_start(string name)
		{
			return get_attribute_start(get_attribute_index(name));
		}

		public int get_attribute_size(int index)
		{
			return index >= 0 && index < number_attributes ? attribute_sizes[index] : -1;
		}

		public int get_attribute_size(string name)
		{
			return get_attribute_size(get_attribute_index(name));
		}

		public bool remove_attribute(int index)
		{
			if (index < 0 || index >= number_attributes) return false;

			number_attributes--;

			if (number_attributes == 0)
			{
				clean_attributes();
				return true;
			}

			attributes.RemoveAt(index);
			attribute_starts.RemoveAt(index);
			attribute_sizes.RemoveAt(index);

			int start = index == 0 ? 0 : attribute_starts[index - 1];
			for (; index < number_attributes; index++)
			{
				attribute_starts[index] = start;
				start += attribute_sizes[index];
			}

			return true;
		}

		public bool remove_attribute(string name)
		{
			return remove_attribute(get_attribute_index(name));
		}
	}
}
