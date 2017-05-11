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

        private Dictionary<String, Object> _CustomData;

        public IReadOnlyDictionary<String, Object> ImmutableCustomData
            => _CustomData;

        #endregion

        #region Constructor(s)

        protected ACustomDataBuilder(Dictionary<String, Object> CustomData = null)
        {

            this._CustomData = CustomData;

        }

        protected ACustomDataBuilder(IReadOnlyDictionary<String, Object> CustomData = null)
        {

            if (CustomData != null && CustomData.Count > 0)
            {

                _CustomData = new Dictionary<String, Object>();

                foreach (var item in CustomData)
                    _CustomData.Add(item.Key, item.Value);

            }

        }

        protected ACustomDataBuilder(IEnumerable<KeyValuePair<String, Object>> CustomData = null)
        {

            if (CustomData != null && CustomData.Any())
            {

                _CustomData = new Dictionary<String, Object>();

                foreach (var item in CustomData)
                {

                    if (!_CustomData.ContainsKey(item.Key))
                        _CustomData.Add(item.Key, item.Value);

                    else
                        _CustomData[item.Key] = item.Value;

                }

            }

        }

        #endregion


        public Boolean HasCustomData
            => _CustomData != null && _CustomData.Count > 0;

        public void AddCustomData(String Key,
                                  Object Value)
        {

            if (_CustomData == null)
                _CustomData = new Dictionary<String, Object>();

            _CustomData.Add(Key, Value);

        }

        public Boolean IsDefined(String Key)
        {

            Object _Value;

            return _CustomData.TryGetValue(Key, out _Value);

        }

        public Object GetCustomData(String Key)
        {

            Object _Value;

            if (_CustomData.TryGetValue(Key, out _Value))
                return _Value;

            return null;

        }

        public T GetCustomDataAs<T>(String Key)
        {

            Object _Value;

            if (_CustomData.TryGetValue(Key, out _Value))
                return (T)_Value;

            return default(T);

        }


        public void IfDefined(String Key,
                              Action<Object> ValueDelegate)
        {

            if (ValueDelegate == null)
                return;

            Object _Value;

            if (_CustomData.TryGetValue(Key, out _Value))
                ValueDelegate(_Value);

        }

        public void IfDefinedAs<T>(String Key,
                                   Action<T> ValueDelegate)
        {

            if (ValueDelegate == null)
                return;

            Object _Value;

            if (_CustomData.TryGetValue(Key, out _Value))
                ValueDelegate((T)_Value);

        }

    }

}
