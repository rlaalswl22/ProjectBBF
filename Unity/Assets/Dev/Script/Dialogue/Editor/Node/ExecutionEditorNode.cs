using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DS.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using DS.Core;
using UnityEditor;

namespace DS.Editor
{
    [DialogueNodeType("Add Execution", "Execution")]
    public class ExecutionEditorNode : DialogueEditorNode
    {
        private readonly Vector2 NodeSize = new Vector2(150, 200);
        private readonly DialogueGraphView _dialogueGraphView;


        #region Main Container
        private ExecutionNodeData _data = new();

        private ObjectField _objField;
        private List<VisualElement> _argumentElements = new(2);

        public override string GUID
        {
            get => _data.GUID;
            set => _data.GUID = value;
        }

        public override string NodeTitle
        {
            get => _data.NodeTitle;
            set => _data.NodeTitle = value;
        }

        public override string TypeName
        {
            get => _data.TypeName;
            set => _data.TypeName = value;
        }

        public override Vector2 Position
        {
            get => _data.Position;
            set => _data.Position = value;
        }

        #endregion

        public ExecutionEditorNode(DialogueGraphView dialogueGraphView, string name)
        {
            title = name;
            NodeTitle = name;
            TypeName = name;
            GUID = Guid.NewGuid().ToString();

            _dialogueGraphView = dialogueGraphView;
        }

        public override void Initialize(Vector2 position)
        {
            Position = position;

            var inputPort = GeneratePort(Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Connection";
            inputContainer.Add(inputPort);

            var outputPort = GeneratePort(Direction.Output);
            outputPort.portName = "Next";
            outputContainer.Add(outputPort);

            LoadStyleSheet();

            AddTitleTextField();
            ObjectFieldInit();

            RefreshExpandedState();
            RefreshPorts();

            _dialogueGraphView.AddElement(this);
            SetPosition(new Rect(position, NodeSize));
        }

        private void ObjectFieldInit()
        {
            _objField = AddObjectField(obj =>
            {
                _data.Descriptor = obj.newValue as ExecutionDescriptor;
                UpdateObjectParam();
                ArgsUpdate();

            }, "output-objectField");
        }

        private void UpdateObjectParam()
        {
            foreach (VisualElement element in _argumentElements)
            {
                mainContainer.Remove(element);
            }
            RefreshExpandedState();
            _argumentElements.Clear();
            
            if (_data.Descriptor is null)
            {
                return;
            }
            
            var types = _data.Descriptor.GetArgumentTypes();
                

            foreach (var type in types)
            {
                if (type == typeof(int))
                {
                    var t = AddIntField((e) => { }, "", "int");
                    _argumentElements.Add(t);
                }
                else if (type == typeof(float))
                {
                    var t = AddFloatField((e) => { }, "", "float");
                    _argumentElements.Add(t);
                }
                else if (type == typeof(string))
                {
                    var t = AddTextField((e) => { }, "", "text");
                    _argumentElements.Add(t);
                }
            }
        }


        public override DialogueNodeData ToSerializedData()
        {
            Position = GetPosition().position;
            
            _data.Warps = _argumentElements.Select(x =>
            {
                switch (x)
                {
                    case IntegerField intField:
                        return new ExecutionNodeData.Warp()
                        {
                            Target = "Int",
                            IntValue = intField.value
                        };
                    case FloatField floatField:
                        return new ExecutionNodeData.Warp()
                        {
                            Target = "Float",
                            FloatValue = floatField.value
                        };
                    case TextField stringField:
                        return new ExecutionNodeData.Warp()
                        {
                            Target = "String",
                            StringValue = stringField.value
                        };
                    default:
                        return null;
                }
            }).ToArray();
            
            return _data;
        }

        private void ArgsUpdate()
        {
            if (_data is null) return;
            if (_data.Warps is null) return;
            var enumerator = _data.Warps.GetEnumerator();

            foreach (VisualElement argumentElement in _argumentElements)
            {
                if (enumerator.MoveNext() == false) break;

                var warp = enumerator.Current as ExecutionNodeData.Warp;

                if (warp is null) return;
                
                switch (argumentElement)
                {
                    case IntegerField intField:
                        intField.value = warp.IntValue;
                        break;
                    case FloatField floatField:
                        floatField.value = warp.FloatValue;
                        break;
                    case TextField stringField:
                        stringField.value = warp.StringValue;
                        break;
                    default:
                        break;
                }
            }

        }
        
        public override void FromSerializedData(DialogueNodeData data, DialogueContainer containerCache)
        {
            if (data is not ExecutionNodeData myData) return;
            
            _data = myData;

            _objField.value = _data.Descriptor;

            UpdateObjectParam();
            ArgsUpdate();
            
            _dialogueGraphView.AddElement(this);
            RefreshExpandedState();
            RefreshPorts();
        }
    }
}