const routes = [
    { path: '/',name:'Home', component: ()=> import('../views/Home.vue'),meta: { requiresAuth: false } },
    { path: '/products/:tag', name: 'Product', component: () => import('../views/product.vue'), meta: { requiresAuth: false } },
    { path: '/product/:id',name:'SingleProduct', component:()=> import('../views/SingleProduct.vue'),meta: { requiresAuth: false } },
    { path: '/recommend',name:'Recommend', component:()=> import('../views/Recommend.vue'),meta: { requiresAuth: false } },
    { path: '/checkout',name:'Checkout', component:()=> import('../views/Checkout.vue'),meta: { requiresAuth: true } },
    { path: '/account',name: 'Account', component:() => import('../views/Account.vue'),meta: { requiresAuth: true },children: [
      { path: 'orders',component: () => import('../components/Orders.vue')},
      { path: 'MemberProfile',component: () => import('../components/MemberProfile.vue')},
      { path: 'MemberCollection',component: () => import('../components/MemberCollection.vue')},
      { path: 'MemberAddresses',component: () => import('../components/MemberAddresses.vue')},
      { path: 'responses',component: () => import('../components/QResponses.vue')},
    ]},
    { path: '/Blog',name:'Blog', component:()=> import('../views/Blog.vue'),meta: { requiresAuth: false } },
    { path: '/Blog/EssaysBlog',name:'EssaysBlog', component:()=> import('../views/EssaysBlog.vue'),meta: { requiresAuth: false } },
    { path: '/Blog/EssaysBlog/:authorName',name:'AuthorPage', component:()=> import('../views/AuthorPage.vue'),meta: { requiresAuth: false } },
    { path: '/Blog/VideoBlog', name: 'VideoBlog', component: () => import('../views/VideoBlog.vue'), meta: { requiresAuth: false } },
    { path: '/VideoBlog/:id', name: 'SingleVideo', component: () => import('../views/SingleVideo.vue'), meta: { requiresAuth: false } },
    { path: '/EssaysBlog/:id', name: 'SingleEssay', component: () => import('../views/SingleEssay.vue') , meta: { requiresAuth: false } },
    { path: '/member', name: 'member', component: () => import('../views/Member.vue'), meta: { requiresAuth: false } },
    { path: '/login',name:'Login', component:()=> import('../views/Login.vue'),meta: { requiresAuth: false } },
    { path: '/OrderComplete', name: 'OrderComplete', component: () => import('../views/OrderComplete.vue'), meta: { requiresAuth: false } },
    { path: '/Questions', name: 'Questions', component: () => import('../views/FAQ.vue'), meta: { requiresAuth: false } },
    { path: '/ServerChatRoom', name: 'ServerChatRoom', component: () => import('../views/ChatRoom_server.vue'), meta: { requiresAuth: false } },
    { path: '/MemberForgetPasswordEmail/:Account/:MailCode', name: 'MemberForgetPasswordEmail', component: () => import('../views/MemberForgetPasswordEmail.vue'), meta: { requiresAuth: false } },
    { path: '/ForgetPasswordEmail', name: 'ForgetPasswordEmail', component: () => import('../views/ForgetPasswordEmail.vue'), meta: { requiresAuth: false } },

  ]
export default routes