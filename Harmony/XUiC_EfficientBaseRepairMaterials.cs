using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XUiC_EfficientBaseRepairMaterials : XUiController
{
    public string[] MaterialNames;

    private int[] weights;

    public TileEntityEfficientBaseRepair tileEntity;

    private XUiController[] materialSprites;

    private XUiController[] materialWeights;

    private Color baseTextColor;

    private Color validColor = Color.green;

    private Color invalidColor = Color.red;

    public override void Init()
    {
        base.Init();
        materialSprites = GetChildrenById("material");
        materialWeights = GetChildrenById("weight");
        if (materialWeights[0] != null)
        {
            baseTextColor = ((XUiV_Label)materialWeights[0].ViewComponent).Color;
        }

        weights = null;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        // MaterialNames = Array.Empty<string>();

        // for (int i = 0; i < MaterialNames.Length; i++)
        // {
        //     string text = XUi.UppercaseFirst(MaterialNames[i]);
        //     if (Localization.Exists("lbl" + MaterialNames[i]))
        //     {
        //         text = Localization.Get("lbl" + MaterialNames[i]);
        //     }
        //     ((XUiV_Label)materialTitles[i].ViewComponent).Text = text + ":";
        // }
        UpdateMaterials();
    }

    public override void Update(float _dt)
    {
        if (windowGroup.isShowing)
        {
            UpdateMaterials();
            base.Update(_dt);
        }
    }

    private void UpdateMaterials()
    {
        int index = 0;

        if(tileEntity == null)
            return;

        if (tileEntity.requiredMaterials == null)
            return;

        Dictionary<string, int> itemsDict = tileEntity.ItemsToDict();

        foreach (KeyValuePair<string, int> entry in tileEntity.requiredMaterials)
        {
            string text = Localization.Get(entry.Key);

            ItemClass materialItemValue = ItemClass.GetForId(ItemClass.GetItem(entry.Key).type);

            XUiV_Sprite sprite = (XUiV_Sprite)materialSprites[index].ViewComponent;
            // sprite.IsVisible = true;
            // sprite.SetSpriteImmediately(materialItemValue.GetIconName());
            sprite.ParseAttribute("sprite", materialItemValue.GetIconName(), materialSprites[index]);

            int availbableMaterialsCount = itemsDict.ContainsKey(entry.Key) ? itemsDict[entry.Key] : 0;
            int requiredMaterialsCount = entry.Value;

            ((XUiV_Label)materialWeights[index].ViewComponent).Text = $"{availbableMaterialsCount} / {requiredMaterialsCount}";

            index++;
        }
    }

    private void ResetWeightColors()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            ((XUiV_Label)materialWeights[i].ViewComponent).Color = baseTextColor;
        }
    }

    public void SetMaterialWeights(ItemStack[] stackList)
    {
        for (int i = 3; i < stackList.Length; i++)
        {
            if (weights != null && stackList[i] != null)
            {
                weights[i - 3] = stackList[i].count;
            }
        }
        // onForgeValuesChanged();
    }

    public override bool ParseAttribute(string name, string value, XUiController _parent)
    {
        bool flag = base.ParseAttribute(name, value, _parent);
        if (!flag)
        {
            switch (name)
            {
                case "materials":
                    // if (value.Contains(","))
                    // {
                    // 	MaterialNames = value.Replace(" ", "").Split(',', StringSplitOptions.None);
                    // 	weights = new int[MaterialNames.Length];
                    // }
                    // else
                    // {
                    // 	MaterialNames = new string[1] { value };
                    // }
                    MaterialNames = new string[1] { value };
                    return true;
                case "valid_materials_color":
                    validColor = StringParsers.ParseColor32(value);
                    return true;
                case "invalid_materials_color":
                    invalidColor = StringParsers.ParseColor32(value);
                    return true;
                default:
                    return false;
            }
        }
        return flag;
    }

    private float calculateWeightOunces(int materialIndex)
    {
        return weights[materialIndex];
    }

    private float calculateWeightPounds(int materialIndex)
    {
        return calculateWeightOunces(materialIndex) / 16f;
    }
}
