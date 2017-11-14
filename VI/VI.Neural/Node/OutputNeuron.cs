﻿using VI.Neural.ANNOperations;
using VI.Neural.Layer;
using VI.Maths.Random;
using System;
using VI.NumSharp.Arrays;

namespace VI.Neural.Node
{
    [Obsolete("Use 'SupervisedNeuron' or 'UnsupervisedNeuron' class with the correct learning mode")]
    public class OutputNeuron : INeuron
    {
        private ActivationLayer _layer;
        private static ThreadSafeRandom _tr = new ThreadSafeRandom();
        private readonly AnnBasicOperations _ann;

        public int NodesSize => _layer.Size;
        public int Connections => _layer.ConectionsSize;

        public ILayer Nodes => _layer;

        public OutputNeuron(int nodeSize, int connectionSize, float learningRate, AnnBasicOperations operations)
        {
            _layer = new ActivationLayer(nodeSize, connectionSize);
            _ann = operations;

            _layer.KnowlodgeMatrix = new Array2D<float>(nodeSize, connectionSize);

            _layer.LearningRate = learningRate;
            _layer.CachedLearningRate = learningRate;

            _layer.OutputVector = new Array<float>(nodeSize);

            _layer.BiasVector = new Array<float>(nodeSize);
            for (var i = 0; i < nodeSize; i++)
            {
                _layer.BiasVector[i] = 1;
            }
        }
        
        public Array<float> Output(Array<float> inputs)
        {
            _ann.FeedForward(_layer, inputs);
            return _layer.OutputVector;
        }
        
        public Array<float> Learn(float[] inputs, Array<float> error)
        {
            using (var i = new Array<float>(inputs))
            {
                _ann.BackWardDesired(_layer, error);
                _ann.BackWardError(_layer);
                _ann.ErrorGradient(_layer, i);
                _ann.UpdateWeight(_layer);
                _ann.UpdateBias(_layer);
                return _layer.ErrorWeightVector;
            }
        }
        public Array<float> Learn(Array<float>  inputs, float[] error)
        {
            using (var e = new Array<float>(error))
            {
                _ann.BackWardDesired(_layer, e);
                _ann.BackWardError(_layer);
                _ann.ErrorGradient(_layer, inputs);
                _ann.UpdateWeight(_layer);
                _ann.UpdateBias(_layer);
                return _layer.ErrorWeightVector;
            }
        }
        public Array<float> Learn(Array<float>  inputs, Array<float> error)
        {
            _ann.BackWardDesired(_layer, error);
            _ann.BackWardError(_layer);
            _ann.ErrorGradient(_layer, inputs);
            _ann.UpdateWeight(_layer);
            _ann.UpdateBias(_layer);
            return _layer.ErrorWeightVector;
        }

        public void Synapsis(int node, int connection)
        {
            _layer.KnowlodgeMatrix[node, connection] = (float)_tr.NextDouble();
        }
        public void Synapsis(int node, int connection, float w)
        {
            _layer.KnowlodgeMatrix[node, connection] = w;
        }
    }
}
