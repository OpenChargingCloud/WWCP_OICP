/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    public abstract class ACustomDataBuilder : ICustomDataBuilder
    {

        #region Data

        public Dictionary<String, Object>  CustomData   { get; }

        #endregion

        #region Constructor(s)

        protected ACustomDataBuilder(Dictionary<String, Object> CustomData = null)
        {

            this.CustomData = CustomData ?? new Dictionary<String, Object>();

        }

        protected ACustomDataBuilder(IReadOnlyDictionary<String, Object> CustomData = null)
        {

            this.CustomData = new Dictionary<String, Object>();

            if (CustomData != null && CustomData.Count > 0)
                foreach (var item in CustomData)
                    this.CustomData.Add(item.Key, item.Value);

        }

        protected ACustomDataBuilder(IEnumerable<KeyValuePair<String, Object>> CustomData = null)
        {

            this.CustomData = new Dictionary<String, Object>();

            if (CustomData != null && CustomData.Any())
                foreach (var item in CustomData)
                {

                    if (!this.CustomData.ContainsKey(item.Key))
                        this.CustomData.Add(item.Key, item.Value);

                    else
                        this.CustomData[item.Key] = item.Value;

                }

        }

        #endregion


        public Boolean HasCustomData
            => CustomData != null && CustomData.Count > 0;

        public void SetCustomData(String Key,
                                  Object Value)
        {

            if (!CustomData.ContainsKey(Key))
                CustomData.Add(Key, Value);

            else
                CustomData[Key] = Value;

        }

        public Boolean IsDefined(String Key)
            => CustomData.TryGetValue(Key, out Object _Value);

        public Object GetCustomData(String Key)
        {

            if (CustomData.TryGetValue(Key, out Object _Value))
                return _Value;

            return null;

        }

        public T GetCustomDataAs<T>(String Key)
        {

            try
            {

                if (CustomData.TryGetValue(Key, out Object _Value))
                    return (T)_Value;

            }
            catch (Exception e)
            { }

            return default(T);

        }


        public void IfDefined(String          Key,
                              Action<Object>  ValueDelegate)
        {

            if (ValueDelegate == null)
                return;

            if (CustomData.TryGetValue(Key, out Object _Value))
                ValueDelegate(_Value);

        }

        public void IfDefinedAs<T>(String     Key,
                                   Action<T>  ValueDelegate)
        {

            if (ValueDelegate == null)
                return;

            try
            {

                if (CustomData.TryGetValue(Key, out Object _Value))
                    ValueDelegate((T)_Value);

            }
            catch (Exception e)
            { }

        }

    }

}
