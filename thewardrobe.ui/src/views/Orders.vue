<template>
  <section>
    <VPageHeader title="My Orders" @back="$router.back()" />
    <ul class="flex justify-evenly w-full mx-auto py-4 mb-4">
      <li
        v-for="tab in tabs"
        :key="tab.tabName"
        v-bind:class="{
          'text-green-400 border-b-2 border-green-400 pb-1':
            currentTab === tab.tabName,
        }"
      >
        <button
          class="hover:text-green-400"
          @click="
            () => {
              currentTab = tab.tabName;
              $router.push(tab.path).catch((err) => {});
            }
          "
        >
          {{ tab.tabName }}
        </button>
      </li>
    </ul>
    <main class="bg-gray-800 px-2 py-4 md:p-8 w-11/12 lg:w-3/5 mx-auto rounded">
      <OrderList :isIncoming="currentTab === 'Incoming'" />
    </main>
  </section>
</template>

<script>
import VPageHeader from "../components/VPageHeader.vue";
import OrderList from "../components/Order/OrderList.vue";

export default {
  components: {
    VPageHeader,
    OrderList,
  },
  data() {
    return {
      currentTab: "Incoming",
      tabs: [
        { tabName: "Incoming", path: "/orders/incoming" },
        { tabName: "Outgoing", path: "/orders/outgoing" },
      ],
    };
  },
  mounted() {
    if (this.$route.path === "/orders/outgoing") {
      this.currentTab = "Outgoing";
    }
  },
};
</script>

<style scoped>
ul {
  font-family: "Josefin Sans", sans-serif;
}
</style>
