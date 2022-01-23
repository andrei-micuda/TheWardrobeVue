<template>
  <section>
    <ul class="flex justify-evenly w-11/12 mx-auto py-4">
      <li
        v-for="tab in tabs" :key="tab.tabName"
        v-bind:class="{'text-green-400 border-b-2 border-green-400 pb-1': (currentTab === tab.tabName)}"
        >
      <button class="hover:text-green-400" @click="() => {currentTab = tab.tabName}">{{ tab.tabName }}</button>
    </li>
    </ul>
    <main>
      <TheBuyingTab v-if="currentTab === 'Buying'" />
      <TheSellingTab v-if="currentTab === 'Selling'" />
    </main>
  </section>
</template>

<script>
import TheBuyingTab from './TheBuyingTab.vue';
import TheSellingTab from './TheSellingTab.vue';

export default {
  components: {
    TheBuyingTab,
    TheSellingTab
  },
  data() {
    return {
      currentTab: 'Buying',
      tabs: [
        {tabName: "Buying"},
        {tabName: "Selling"},
      ]
    }
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
ul { 
  font-family: 'Josefin Sans', sans-serif;
}
</style>
