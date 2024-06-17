const NameApp = {
    data() {
        return {
            FullName: "",
            CPF: "",
            DateOfBirth: "",
            Address: "",
            Contact: ["", ""]
        }
    },
    methods: {
        submitForm(e) {
            e.preventDefault();

            console.log("Nome Completo: ", this.FullName);
            console.log("CPF: ", this.CPF);
            console.log("Data de Nascimento: ", this.DateOfBirth);
            console.log("Endereço: ", this.Address);
            console.log("Telefone: ", this.Contact[0]);
            console.log("E-mail: ", this.Contact[1]);
        }
    }
}
Vue.createApp(NameApp).mount('#app');