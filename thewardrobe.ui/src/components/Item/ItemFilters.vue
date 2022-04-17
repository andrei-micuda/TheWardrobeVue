<template>
  <div class="bg-gray-800 border border-gray-500 p-5 rounded">
    <div class="flex justify-between items-center">
      <span>FILTERS</span>
      <a-button @click="getFilteredData">Apply</a-button>
    </div>

    <a-divider class="bg-gray-500 my-4" />

    <a-form class="text-left" >
      <a-form-item label="Gender">
        <a-radio-group v-model="selectedGender" class="flex justify-center" button-style="solid">
          <a-radio-button value="all">
            All
          </a-radio-button>
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
      <a-form-item label="Brands">
        <a-tree-select
          v-model="selectedBrands"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :tree-data="brandData"
          tree-checkable
          :show-checked-strategy="SHOW_PARENT"
          show-search
          allow-clear
          placeholder="Please select a brand"
        >
        </a-tree-select>
      </a-form-item>
      <a-form-item label="Categories">
        <a-tree-select
          v-model="selectedCategories"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :tree-data="categoryData"
          tree-checkable
          :show-checked-strategy="SHOW_PARENT"
          show-search
          allow-clear
          placeholder="Please select a category"
        >
        </a-tree-select>
      </a-form-item>
      <a-form-item label="Sizes">
        <a-tree-select
          v-model="selectedSizes"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :tree-data="sizeData"
          tree-checkable
          :show-checked-strategy="SHOW_PARENT"
          show-search
          allow-clear
          placeholder="Please select a size"
        >
        </a-tree-select>
      </a-form-item>
      <a-form-item label="Price Range">
        <a-slider range
          v-model="selectedPriceRange"
          :min="initialMinPrice"
          :max="initialMaxPrice"
          :disabled="initialMinPrice === initialMaxPrice"
        />
        <a-row type="flex" justify="space-between" class="text-gray-100">
          <a-col :span="6">
            <span>{{selectedPriceRange[0]}}</span>
          </a-col>
          <a-col :span="6" class="text-right">
            <span>{{selectedPriceRange[1]}}</span>
          </a-col>
        </a-row>
        <!-- <a-row type="flex" justify="space-between">
          <a-col :span="6">
            <a-input v-model="selectedPriceRange[0]" size="small" placeholder="Min" @pressEnter="onMinPriceChange" />
          </a-col>
          <a-col :span="6">
            <a-input v-model="selectedPriceRange[1]" size="small" placeholder="Max" @pressEnter="onMaxPriceChange" />
          </a-col>
        </a-row> -->
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
  /* eslint-disable no-unused-vars */
  import {TreeSelect} from "ant-design-vue";

  import constData from '../../const';
  import { haveCommonElements } from '../../helpers/arrayHelpers';

  const SHOW_PARENT = TreeSelect.SHOW_PARENT;

  const categories = {
    clothing: constData.clothingCategories
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
    "clothing&acc": [...constData.clothingSizes.map(cat => {
          return {title: cat, value: cat, key: cat}
          })
    ],
    "footwear": [...constData.footwearSizes.map(cat => {
        return {title: cat, value: cat, key: cat}
        })
    ],
  }

  const brandData = [...constData.brands.map(brand => {
    return {title: brand, value: brand}
  })];
  export default {
    props: {
      initialMinPrice: {
        type: Number
      },
      initialMaxPrice: {
        type: Number
      },
    },
    data() {
      return {
        brandData,
        categoryData,
        SHOW_PARENT,
        selectedBrands: [],
        selectedCategories: [],
        selectedGender: "all",
        selectedSizes: [],
        selectedPriceRange: [this.initialMinPrice, this.initialMaxPrice]
      }
    },
    computed: {
      includesClothing() {
        // check if all clothing categories have been selected
        if(this.selectedCategories.includes('Clothing'))
          return true;
        return haveCommonElements(categories['clothing'], this.selectedCategories);
      },
      includesFootwear() {
        return (this.selectedCategories.includes('Footwear'));
      },
      includesAccessory() {
        return (this.selectedCategories.includes('Accessories'));
      },
      sizeData() {
        let res = [];
        if(this.includesClothing || this.includesAccessory) {
          res = res.concat(sizes["clothing&acc"]);
        }

        if(this.includesFootwear) {
          res = res.concat(sizes["footwear"]);
        }
        return res;
      }
    },
    methods: {
      onMinPriceChange(e) {
        const { value } = e.target;
        const reg = /^[0-9]*(\.[0-9]*)?$/;
        if ((!isNaN(value) && reg.test(value)) || value === '') {
          this.selectedPriceRange[0] = value;
        }
      },
      onMaxPriceChange(e) {
        console.log(e)
        const { value } = e.target;
        const reg = /^[0-9]*(\.[0-9]*)?$/;
        if ((!isNaN(value) && reg.test(value)) || value === '') {
          console.log("Updating...")
          this.selectedPriceRange[1] = parseInt(value);
        }
      },
      getFilteredData() {
        const params = {
          brands: this.selectedBrands,
          categories: this.selectedCategories,
          sizes: this.selectedSizes,
          minPrice: this.selectedPriceRange[0],
          maxPrice: this.selectedPriceRange[1]
        }

        if(this.selectedGender !== 'all')
          params.genders = this.selectedGender;

        this.$emit("filterData", params);
      }
    },
  }
</script>

<style lang="postcss" scoped>
  .ant-form-item {
    @apply mb-2 !important;
  }
</style>