<template>
  <section>
    <ul id="BuySellTabs" class="flex justify-evenly w-full mx-auto py-4 mb-4">
      <li
        v-for="tab in tabs" :key="tab.tabName"
        v-bind:class="{'text-green-400 border-b-2 border-green-400 pb-1': (currentTab === tab.tabName)}"
        >
      <button class="hover:text-green-400" @click="() => {
        currentTab = tab.tabName;
        $router.push(tab.path);
        }">{{ tab.tabName }}</button>
    </li>
    </ul>
    <main>
      <TheBuyingTab v-if="currentTab === 'Buying'" />
      <TheSellingTab v-if="currentTab === 'Selling'" />
    </main>
  </section>
</template>

<script>
import TheBuyingTab from '../components/TheBuyingTab.vue';
import TheSellingTab from '../components/Selling/TheSellingTab.vue';

export default {
  components: {
    TheBuyingTab,
    TheSellingTab
  },
  data() {
    return {
      currentTab: 'Buying',
      tabs: [
        {tabName: "Buying", path: '/buy'},
        {tabName: "Selling", path: '/sell'},
      ]
    }
  },
  mounted () {
    if(this.$route.path === '/sell')
    {
      this.currentTab = 'Selling';
    }
    console.log("Mounting home...");
  },
}
</script>

<style scoped>
ul { 
  font-family: 'Josefin Sans', sans-serif;
}
</style>
