<template>
  <div class="flex justify-between items-center p-4 bg-gray-700 rounded">
    <div>
      <p v-if="isIncoming">
        Order from <span class="font-bold">{{ order.seller }}</span>
      </p>
      <p v-else>
        Order to <span class="font-bold">{{ order.buyer }}</span>
      </p>
      <div class="flex flex-col md:flex-row">
        <p>Placed at: {{ order.whenPlaced.format("DD/MM/YYYY HH:mm") }}</p>
        <p class="mx-2 hidden md:block">|</p>
        <p>
          Total: <span class="font-bold">{{ order.total }} RON</span>
        </p>
      </div>
    </div>
    <div class="flex flex-col items-end md:flex-row">
      <div class="md:mr-6 flex justify-end md:items-center py-2 md:p-2 w-36">
        <p>
          Status:
          <span :class="orderStatusClass[order.status]">{{
            orderStatus[order.status]
          }}</span>
        </p>
      </div>
      <router-link :to="{ name: 'order', params: { orderId: order.id } }">
        <a-button>details</a-button>
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    isIncoming: {
      type: Boolean,
      default: false,
    },
    order: {
      type: Object,
    },
  },
  data() {
    return {
      orderStatus: [
        "Pending",
        "In Progress",
        "Completed",
        "Cancelled",
        "Declined",
      ],
      orderStatusClass: [
        "pending",
        "inProgress",
        "completed",
        "cancelled",
        "declined",
      ],
    };
  },
};
</script>

<style lang="postcss" scoped>
.pending {
  @apply text-gray-400 !important;
}

.inProgress {
  @apply text-blue-300 !important;
}

.completed {
  @apply text-green-400 !important;
}

.cancelled,
.declined {
  @apply text-red-400 !important;
}
</style>
