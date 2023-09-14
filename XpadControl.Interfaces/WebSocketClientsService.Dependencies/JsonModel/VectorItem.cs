﻿using System.Text.Json.Serialization;

namespace XpadControl.JsonModel
{
    public class VectorItem
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("z")]
        public float Z { get; set; }
    }
}
