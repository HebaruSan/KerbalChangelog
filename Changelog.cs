﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalChangelog
{
    public class Changelog
    {
        public string modName { get; private set; }
        bool showCL = true;
        List<ChangeSet> changeSets = new List<ChangeSet>();
        public ChangelogVersion highestVersion
        {
            get
            {
                changeSets.Sort();
                return changeSets[0].version;
            }
        }

        public Changelog(string mn, bool show, List<ChangeSet> cs)
        {
            modName = mn;
            showCL = show;
            changeSets = cs;
        }

        public Changelog(ConfigNode cn, string cfgDirName)
        {
            string _modname = "";
            if (!cn.TryGetValue("modName", ref _modname))
            {
                Debug.Log("[KCL] Missing mod name for changelog file in directory: " + cfgDirName);
                Debug.Log("[KCL] Continuing using directory name as mod name...");
                modName = cfgDirName;
            }
            else
            {
                modName = _modname;
            }

            if (!cn.TryGetValue("showChangelog", ref showCL))
            {
                Debug.Log("[KCL] \"showChangelog\" field does not exist in mod ");
                Debug.Log("[KCL] Assuming [true] to show changelog, adding field to changelog...");
                if (!cn.SetValue("showChangelog", false, true)) //creates a new field for the viewing status, setting it to false
                {
                    Debug.Log("[KCL] Unable to create 'showChangelog' in directory " + cfgDirName);
                }
            }
            foreach(ConfigNode vn in cn.GetNodes("VERSION"))
            {
                changeSets.Add(new ChangeSet(vn, cfgDirName));
            }
        }

        public override string ToString()
        {
            string ret = modName + "\n";
            foreach(ChangeSet cs in changeSets)
            {
                ret += cs.ToString();
            }
            return ret;
        }
    }
}