<template>
  <div>
    <a-form :form="form" v-bind="formItemLayout">
      <a-form-item label="Product Name">
        <a-input
          v-decorator="[
            'productName',
            {
              rules: [
                {
                  required: true,
                  message: 'Please provide a product name!',
                },
              ],
            },
          ]"
        />
      </a-form-item>
      <a-form-item label="Gender">
        <a-radio-group default-value="unisex" button-style="solid">
          <a-radio-button value="unisex">
            Unisex
          </a-radio-button>
          <a-radio-button value="male">
            Male
          </a-radio-button>
          <a-radio-button value="female">
            Female
          </a-radio-button>
        </a-radio-group>
      </a-form-item>
      <a-form-item label="Category">
        <a-tree-select
          v-model="selectedCategory"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :tree-data="categoryData"
          placeholder="Please select"
        >
        </a-tree-select>
      </a-form-item>
      <a-form-item label="Size">
        <a-tree-select
          v-model="selectedSize"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :tree-data="sizeData"
          placeholder="Please select"
          tree-default-expand-all
        >
        </a-tree-select>
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
  const categories = {
    clothing: ["Anorak", "Blazer", "Blouse", "Bomber", "Button-Down", "Cardigan", "Flannel", "Halter", "Henley", "Hoodie", "Jacket", "Jersey", "Parka", "Peacoat", "Poncho", "Sweater", "Tank", "Tee", "Top", "Turtleneck", "Capris", "Chinos", "Culottes", "Cutoffs", "Gauchos", "Jeans", "Jeggings", "Jodhpurs", "Joggers", "Leggings", "Sarong", "Shorts", "Skirt", "Sweatpants", "Sweatshorts", "Trunks", "Caftan", "Cape", "Coat", "Coverup", "Dress", "Jumpsuit", "Kaftan", "Kimono", "Nightdress", "Onesie", "Robe", "Romper", "Shirtdress", "Sundress"]
  };

  const categoryData = [
    {
      title: 'Clothing',
      value: 'Clothing',
      key: 'Clothing',
      selectable: false,
      children: [
        ...categories["clothing"].map(cat => {
          return {title: cat, value: cat, key: cat}
          })
      ],
    },
    {
      title: 'Footwear',
      value: 'Footwear',
      key: 'Footwear'
    },
    {
      title: "Accessories",
      value: "Accessories",
      key: "Accessories"
    }
  ];

  const sizes = {
    "clothing&acc": [...["XS", "S", "M", "L", "XL", "one-size"].map(cat => {
          return {title: cat, value: cat, key: cat}
          })
    ],
    "footwear": [...["37", "38", "39", "40", "41", "42", "43", "44", "45", "46"].map(cat => {
        return {title: cat, value: cat, key: cat}
        })
    ],
  }

  export default {
    props: {
      predictedCategory: {
        type: String,
      },
    },
    data() {
      return {
        categoryData,
        selectedCategory: null,
        selectedSize: null,
        formItemLayout: {
          labelCol: { span: 6 },
          wrapperCol: { span: 18 },
        },
      }
    },
    computed: {
      isClothing() {
        return categories["clothing"].includes(this.selectedCategory);
      },
      isFootwear() {
        return (this.selectedCategory === "Footwear");
      },
      isAccessory() {
        return (this.selectedCategory === "Accessories");
      },
      sizeData() {
        if(this.isClothing || this.isAccessory) {
          return sizes["clothing&acc"];
        }
        else {
          return sizes["footwear"];
        }
      }
    },
    beforeCreate() {
      this.form = this.$form.createForm(this, { name: 'addItem' });
    },
    mounted() {
      this.selectedCategory = this.predictedCategory;
    }
  }
</script>

<style lang="postcss">

</style>