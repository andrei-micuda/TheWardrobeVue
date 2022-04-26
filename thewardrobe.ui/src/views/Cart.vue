<template>
  <div>
    <VPageHeader title="My Cart" @back="$router.back()" />
    <ul v-if="items">
      <li v-for="group in itemGroups.keys()" :key="group">
        {{group}}
      </li>
    </ul>
  </div>
</template>

<script>
  import VPageHeader from '../components/VPageHeader.vue';

  import api from '../api';
  import store from '../store';
  import { groupBy } from '../helpers/arrayHelpers';

  export default {
    data() {
      return {
        items: null
      }
    },
    computed: {
      itemGroups() {
        return groupBy(this.items, i => i.sellerId);
      }
    },
    mounted () {
      api.get(`/api/${store.state.id}/cart`)
        .then(res => {
          this.items = res.data;

          console.log(this.items)
          console.log(groupBy(this.items, i => i.sellerId));
        })
    },
    components: {
      VPageHeader
    }
  }
</script>

<style lang="scss" scoped>

</style>