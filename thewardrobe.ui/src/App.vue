<template>
  <div id="app" class="min-h-screen flex flex-col bg-gray-700">
    <TheNavbar />
    <router-view/>
  </div>
</template>

<script>
import TheNavbar from "./components/TheNavbar.vue"
export default {
  components: {
        TheNavbar
    }
}
</script>

<style lang="postcss">
@import url('https://fonts.googleapis.com/css2?family=Josefin+Sans:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;1,100;1,200;1,300;1,400;1,500;1,600;1,700&family=Libre+Franklin:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap');
* {
  box-sizing: border-box;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  /* text-align: center; */
  color: #fff;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

.anticon {
  @apply text-gray-100 !important;
}

.ant-btn {
  @apply bg-transparent text-gray-100 !important;

  &:hover {
    @apply border-green-400 !important;
  }  
}

.ant-btn-default {
  @apply bg-transparent border-gray-300 text-gray-300 hover:border-green-300 hover:text-gray-700 !important;

  &:hover {
    @apply text-gray-100 !important;
  }
}

.ant-btn-primary {
  @apply bg-transparent border-green-400 text-green-400 hover:bg-green-400 hover:text-gray-100 !important;
  text-shadow: none !important;
}

.ant-btn-danger {
  @apply text-gray-300 border-gray-300
    hover:bg-red-400 hover:border-opacity-0 hover:text-gray-700
    !important;
  text-shadow: none !important;
}

.ant-message {
  @apply mt-14 !important;

  & .anticon {
    top: -2px !important;
  }
}

.ant-spin-dot-item {
  @apply bg-green-400 !important;
}

.ant-radio-inner {
  @apply border-green-400 bg-gray-800 !important;

  &::after {
    @apply bg-green-400 !important;
  }
}

.ant-radio-group-solid .ant-radio-button-wrapper-checked:not(.ant-radio-button-wrapper-disabled) {
  @apply bg-green-400 border-green-400 !important;

  &::before {
    @apply bg-green-400 !important;
  }
}

.ant-radio-button-wrapper:hover:not(.ant-radio-button-wrapper-checked) {
  @apply text-green-400 !important;
}

.ant-radio-button-wrapper {
  @apply bg-transparent border-gray-500 text-gray-100 !important;

  &:not(:first-child)::before {
    @apply bg-gray-500 !important;
  }
}

.ant-form,
.ant-form-item-label > label {
  @apply text-gray-100 !important;
}

.ant-upload-list-item-card-actions .anticon,
.ant-upload-list-item-info {
  @apply text-gray-100 !important;
}

.ant-modal-content {
  @apply overflow-hidden !important;
}

.ant-modal-body {
  @apply bg-gray-600 overflow-hidden !important;
}

.ant-modal-confirm-body .ant-modal-confirm-title,
.ant-modal-confirm-body .ant-modal-confirm-content {
  @apply text-gray-100 !important;
}

.ant-input {
  @apply bg-transparent border-gray-500 text-gray-100 !important;

  &:focus {
    box-shadow: 0 0 0 2px rgba(52, 211, 153, 0.2) !important;
  }
}

.ant-select-tree .ant-select-switcher-icon {
  @apply relative !important;
  top: -3px;
}

.ant-select-selection--multiple {
  max-height: 70px !important;
  @apply overflow-auto bg-transparent border-gray-500 !important;

  & .ant-select-search__field {
    @apply text-gray-100 !important;
  }

  & .ant-select-selection__choice {
    @apply bg-gray-700 border-gray-600 text-gray-100 !important;
  }

  & .ant-select-selection__choice__remove {
    @apply text-gray-100 !important;
  }
}

.ant-select-tree-dropdown {
  @apply bg-gray-700 border-gray-600 !important;

  & li .ant-select-tree-node-content-wrapper {
    @apply text-gray-100 !important;

    &:hover {
      @apply bg-gray-500 !important;
    }
  }

  & .ant-select-dropdown-search {
    @apply bg-gray-700 !important;

    & .ant-select-search__field {
      @apply bg-transparent text-gray-100 !important;
    }
  }
}

.ant-select-tree-checkbox-indeterminate .ant-select-tree-checkbox-inner::after {
  @apply bg-green-500 !important;
}

.ant-select-tree-checkbox-checked .ant-select-tree-checkbox-inner {
  @apply bg-green-500 border-green-500 !important;
}

.ant-select-selection__clear {
  @apply bg-transparent text-gray-100 !important;
}

.ant-slider-rail {
  @apply bg-gray-500 !important;
}

.ant-slider-track {
  @apply bg-green-400 !important;
}

.ant-slider-handle {
  @apply border-green-400 bg-green-400 !important;

  &:focus {
    box-shadow: 0 0 0 5px rgba(52, 211, 153, 0.2) !important;
  }
}

.ant-pagination-item,
.ant-pagination-item-active,
.ant-pagination-item-link {
  @apply bg-transparent !important;
}

.ant-pagination-next:hover .ant-pagination-item-link,
.ant-pagination-prev:hover .ant-pagination-item-link {
  @apply border-green-400 !important;
}

.ant-pagination-item-link .anticon {
  @apply relative;
  top: -3px;
}

.ant-pagination-item {
  &:hover {
    @apply border-green-400 !important;
  }

  & a {
    @apply text-gray-100 !important;
  }
}

.ant-pagination-item-active {
  @apply border-green-400 !important;

  & a {
    @apply text-green-400 !important;
  }
}

.ant-select-open .ant-select-selection,
.ant-select-focused .ant-select-selection {
  @apply border-green-400 !important;
  box-shadow: 0 0 0 2px rgba(52, 211, 153, 0.2) !important;
}

.ant-select-selection {
  @apply bg-transparent text-gray-100 !important;

  &:hover, &:focus, &:focus-visible, &:active {
    @apply border-green-400 !important;
    box-shadow: 0 0 0 2px rgba(52, 211, 153, 0.2) !important;
    outline: none !important;
  }
}

.ant-select-selection-selected-value {
  @apply mr-1 !important;
}

.ant-select-dropdown {
  @apply bg-gray-700 border-2 border-gray-800 !important;
}



.ant-select-dropdown-menu-item {
  @apply bg-gray-700 text-gray-100 !important;

  &:hover {
    @apply bg-gray-600 !important;
  }
}

.ant-input-suffix {
  @apply text-gray-100 !important;
}

.ant-switch {
  @apply bg-transparent border-gray-500 !important;
}

.ant-switch-checked {
  @apply bg-green-400 border-transparent !important;
}

.ant-drawer-header {
  @apply bg-gray-700 border-gray-500 !important;

  & .ant-drawer-title {
    @apply text-gray-100 !important;
  }
}

.ant-drawer-content {
  @apply bg-gray-700 text-gray-100 !important;
}

.ant-modal-header,
.ant-modal-title,
.ant-modal-body,
.ant-modal-footer {
  @apply bg-gray-600 border-gray-500 text-gray-100 !important;
}

.ant-collapse, .ant-collapse-content {
  @apply bg-transparent text-gray-100 !important;
}

.ant-collapse-header {
  @apply text-gray-100 !important;
}

.ant-steps-item-finish {
  & .ant-steps-item-tail::after {
    @apply bg-green-400 !important;
  }

  & .ant-steps-item-icon {
    @apply bg-transparent border-green-400 !important;
  }

  & .ant-steps-item-icon > .ant-steps-icon {
    top: -4px;
  }
}

.ant-steps-item-process {
  & .ant-steps-item-icon {
    @apply bg-green-400 border-green-500 !important;
  }

  & .ant-steps-icon {
    @apply text-gray-800 !important;
  }
}

.ant-steps-item-wait {
  & .ant-steps-item-icon {
    @apply bg-transparent border-gray-100 !important;
  }

  & .ant-steps-icon {
    @apply text-gray-100 !important;
  }
}

.ant-steps-vertical .ant-steps-item-content .ant-steps-item-title,
.ant-steps-vertical .ant-steps-item-content .ant-steps-item-description {
  @apply text-gray-100 !important;
}

/* a {
  @apply hover:text-green-500 !important;
} */
</style>
