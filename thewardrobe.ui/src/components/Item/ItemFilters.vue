<template>
  <div class="bg-gray-800 border border-gray-500 p-5 rounded">
    <div class="flex justify-between items-center">
      <div
        class="flex items-center cursor-pointer"
        :class="{ collapsed: isCollapsed }"
        @click="toggleCollapse"
      >
        <Icon
          icon="dashicons:arrow-down-alt2"
          width="20"
          class="mr-2 transition-all"
        />
        <span>FILTERS</span>
      </div>
      <div>
        <a-button @click="handleClearFilters" class="border-0 text-gray-300"
          >Clear</a-button
        >
        <a-button @click="handleApply" id="ApplyFiltersBtn" type="primary"
          >Apply</a-button
        >
      </div>
    </div>

    <a-divider class="bg-gray-500 my-4" :class="{ hidden: isCollapsed }" />

    <a-form class="text-left" :class="{ hidden: isCollapsed }">
      <a-form-item label="Gender">
        <a-radio-group
          v-model="selectedGender"
          class="flex justify-center"
          button-style="solid"
        >
          <a-radio-button value="all"> All </a-radio-button>
          <a-radio-button value="unisex"> Unisex </a-radio-button>
          <a-radio-button value="male"> Male </a-radio-button>
          <a-radio-button value="female"> Female </a-radio-button>
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
      <a-form-item v-if="selectedMinPrice !== null" label="Price Range">
        <a-slider
          range
          v-model="selectedPriceRange"
          :min="initialMinPrice"
          :max="initialMaxPrice"
          :disabled="initialMinPrice === initialMaxPrice"
        />
        <a-row type="flex" justify="space-between" class="text-gray-100">
          <a-col :span="6">
            <span>{{ selectedPriceRange[0] }}</span>
          </a-col>
          <a-col :span="6" class="text-right">
            <span>{{ selectedPriceRange[1] }}</span>
          </a-col>
        </a-row>
      </a-form-item>
      <a-form-item label="Only favorites?" class="flex space-x-2">
        <a-switch
          :checked="onlyFavorites"
          @change="(v) => (onlyFavorites = v)"
        />
      </a-form-item>
      <a-form-item label="Only available?" class="flex space-x-2">
        <a-switch
          :checked="onlyAvailable"
          @change="(v) => (onlyAvailable = v)"
        />
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
import { TreeSelect } from "ant-design-vue";
import { Icon } from "@iconify/vue2";

import constData from "../../const";
import { haveCommonElements } from "../../helpers/arrayHelpers";

import store from "../../store";

const SHOW_PARENT = TreeSelect.SHOW_PARENT;

const categories = {
  clothing: constData.clothingCategories,
};

const categoryData = [
  {
    title: "Clothing",
    value: "Clothing",
    key: "Clothing",
    selectable: false,
    children: [
      ...categories["clothing"].map((cat) => {
        return { title: cat, value: cat, key: cat };
      }),
    ],
  },
  {
    title: "Footwear",
    value: "Footwear",
    key: "Footwear",
  },
  {
    title: "Accessories",
    value: "Accessories",
    key: "Accessories",
  },
];

const sizes = {
  "clothing&acc": [
    ...constData.clothingSizes.map((cat) => {
      return { title: cat, value: cat, key: cat };
    }),
  ],
  footwear: [
    ...constData.footwearSizes.map((cat) => {
      return { title: cat, value: cat, key: cat };
    }),
  ],
};

const brandData = [
  ...constData.brands.map((brand) => {
    return { title: brand, value: brand };
  }),
];

export default {
  props: {
    initialMinPrice: {
      type: Number,
    },
    initialMaxPrice: {
      type: Number,
    },
  },
  data() {
    return {
      isCollapsed: false,
      brandData,
      categoryData,
      SHOW_PARENT,
      selectedBrands: [],
      selectedCategories: [],
      selectedGender: "all",
      selectedSizes: [],
      selectedMinPrice: this.initialMinPrice,
      selectedMaxPrice: this.initialMaxPrice,
      onlyFavorites: false,
      onlyAvailable: true,
    };
  },
  watch: {
    initialMinPrice(newValue) {
      if (!this.selectedMinPrice) this.selectedMinPrice = newValue;
    },
    initialMaxPrice(newValue) {
      if (!this.selectedMaxPrice) this.selectedMaxPrice = newValue;
    },
  },
  computed: {
    selectedPriceRange: {
      get: function () {
        return [this.selectedMinPrice, this.selectedMaxPrice];
      },
      set: function (newPriceRange) {
        this.selectedMinPrice = newPriceRange[0];
        this.selectedMaxPrice = newPriceRange[1];
      },
    },
    includesClothing() {
      // check if all clothing categories have been selected
      if (
        this.selectedCategories.length === 0 ||
        this.selectedCategories.includes("Clothing")
      )
        return true;
      return haveCommonElements(
        categories["clothing"],
        this.selectedCategories
      );
    },
    includesFootwear() {
      return (
        this.selectedCategories.length === 0 ||
        this.selectedCategories.includes("Footwear")
      );
    },
    includesAccessory() {
      return (
        this.selectedCategories.length === 0 ||
        this.selectedCategories.includes("Accessories")
      );
    },
    sizeData() {
      let res = [
        {
          title: "Other",
          value: "Other",
          key: "Other",
        },
      ];
      if (this.includesClothing || this.includesAccessory) {
        res = res.concat(sizes["clothing&acc"]);
      }

      if (this.includesFootwear) {
        res = res.concat(sizes["footwear"]);
      }

      return [...new Set(res.map((obj) => JSON.stringify(obj)))].map((str) =>
        JSON.parse(str)
      );
    },
  },
  methods: {
    toggleCollapse() {
      this.isCollapsed = !this.isCollapsed;
    },
    onMinPriceChange(e) {
      const { value } = e.target;
      const reg = /^[0-9]*(\.[0-9]*)?$/;
      if ((!isNaN(value) && reg.test(value)) || value === "") {
        this.selectedPriceRange[0] = value;
      }
    },
    onMaxPriceChange(e) {
      const { value } = e.target;
      const reg = /^[0-9]*(\.[0-9]*)?$/;
      if ((!isNaN(value) && reg.test(value)) || value === "") {
        this.selectedPriceRange[1] = parseInt(value);
      }
    },
    handleApply() {
      this.getFilteredData(1);

      store.commit("setItemFilters", {
        brands: this.selectedBrands,
        categories: this.selectedCategories,
        sizes: this.selectedSizes,
        initialMinPrice: this.initialMinPrice,
        initialMaxPrice: this.initialMaxPrice,
        minPrice: this.selectedPriceRange[0],
        maxPrice: this.selectedPriceRange[1],
        onlyFavorites: this.onlyFavorites,
        onlyAvailable: this.onlyAvailable,
      });
    },
    getFilteredData(page = null) {
      const params = {
        brands: this.selectedBrands,
        categories: this.selectedCategories,
        sizes: this.selectedSizes,
        minPrice: this.selectedPriceRange[0],
        maxPrice: this.selectedPriceRange[1],
        onlyFavorites: this.onlyFavorites,
        onlyAvailable: this.onlyAvailable,
      };

      if (page !== null) {
        params.page = page;
        this.$emit("setPaging", page);
      }

      if (this.selectedGender !== "all") params.genders = this.selectedGender;

      this.$emit("filterData", params);
    },
    resetFilters() {
      this.selectedBrands = [];
      this.selectedCategories = [];
      this.selectedGender = "all";
      this.selectedSizes = [];
      this.selectedMinPrice = this.initialMinPrice;
      this.selectedMaxPrice = this.initialMaxPrice;
      this.onlyFavorites = false;
      this.onlyAvailable = true;

      store.commit("setItemFilters", null);
    },
    handleClearFilters() {
      this.resetFilters();
      this.$emit("filtersCleared");
    },
  },
  components: {
    Icon,
  },
  mounted() {
    const itemFilters = store.state.itemFilters;
    if (itemFilters !== null) {
      const {
        brands,
        categories,
        maxPrice,
        minPrice,
        onlyAvailable,
        onlyFavorites,
        sizes,
      } = itemFilters;

      this.selectedBrands = brands;
      this.selectedCategories = categories;
      this.selectedMaxPrice = maxPrice;
      this.selectedMinPrice = minPrice;
      this.onlyAvailable = onlyAvailable;
      this.onlyFavorites = onlyFavorites;
      this.selectedSizes = sizes;
    }
  },
};
</script>

<style lang="postcss" scoped>
.ant-form-item {
  @apply mb-2 !important;
}
</style>
