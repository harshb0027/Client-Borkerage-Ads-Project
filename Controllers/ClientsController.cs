using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab04.Data;
using Lab04.Models;
using Lab04.Models.ViewModels;

namespace Lab04.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MarketDbContext _context;
        List<BrokerageSubscriptionsViewModel> subscriptionTableIncludingembership = new List<BrokerageSubscriptionsViewModel> { };
        public ClientsController(MarketDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        



        public async Task<IActionResult> EditSubscriptions(int id)
        {
            /*
            ClientSubscriptionsViewModel vm = new ClientSubscriptionsViewModel();
            BrokerageSubscriptionsViewModel tmp;

            vm.Client = await _context.Clients.Where(c => c.Id == id).FirstAsync();

            // Join Brokerages to Subscribtions to Client and put all in a variable
            var allBrok = _context.Brokerages.OrderBy(b => b.Title)
                .Include(i => i.Subscriptions)
                .ThenInclude(x => x.Client)
                .ToList();

            var subscriptions01 = _context.Subscriptions.ToList();
            // This is just so we can add to the list later on
            vm.Subscriptions = new List<BrokerageSubscriptionsViewModel> { };
            IEnumerable<BrokerageSubscriptionsViewModel> notMemberList = new List<BrokerageSubscriptionsViewModel> { };

            int i = 0;
            allBrok.ForEach(b => // loop through each Brokerage ( which is joined with subscribtions and clients )
            {
                tmp = new BrokerageSubscriptionsViewModel(); // the View model we need to make and put in the array of the view model that we pass
                tmp.BrokerageId = b.Id; // the brokerage ID of the brokerage we are looping through
                tmp.Title = b.Title;

                //var list= b.subscriptions.Where(s=> s.ClientId == vm.Client.Id).ToList(); // loop through all subscribsions to find all the subscribtions with the client that we have 


                tmp.IsMember = false; // we assume nobody is subscribed unless found otherwise later 

                for (i = 0; i < b.Subscriptions.Count; i++) // loop through each subscription of the table and find which one is related to the client we are looking for
                {
                    if (b.Subscriptions.ElementAt(i).Client.Id == vm.Client.Id)
                        tmp.IsMember = true; // if found the subscription, set that person as a member of this brokerage
                }

                if (tmp.IsMember)
                    vm.Subscriptions = vm.Subscriptions.Concat(new[] { tmp });
                else
                    notMemberList = notMemberList.Concat(new[] { tmp });
            }

            );
            vm.Subscriptions = vm.Subscriptions.Concat(notMemberList);



            return View(vm);
            */
            /*
             * 1->create object of CS View model
             * 2->passing the client details which you find by querying the database client table
             * using that specific id that you rreceived in the parameter
             * 3->fillout the list of subscriptions i.e. in the model
             * 4-> to fill that list first query all the brokerages in your database in the brokerage mtanle 
             * and loop over all those brokerages 
             * for each of the brokerages you are goung to craete an object of brokerage
             * subscription view model
             * and while you are creating that object the isMember should be false
             * 5->add al of these objects to the list in the previous comment (127) Point 05
             * ->turn the isMember for the one the client is a member of:-
             * first query all the subscriptions for that particular client id as a parameter
             * so u will be getting all the rokerages for that client
             * ->have a nested loop over this list of brokerages that you just extracted from subscription table and list 
             * that you have in 127
             * 
             * 
             */
            ClientSubscriptionsViewModel clientSubModel = new ClientSubscriptionsViewModel();
            IEnumerable<BrokerageSubscriptionsViewModel> nonSubscribers= new List<BrokerageSubscriptionsViewModel> { };
           

                
            clientSubModel.Client = await _context.Clients.Where(c => c.Id == id).FirstAsync();
            clientSubModel.Subscriptions = new List<BrokerageSubscriptionsViewModel> { };
            int numBrokerages = _context.Brokerages.Count();
            //query over all the brokerages
            var brokerages = _context.Brokerages.OrderBy(b=> b.Title)
                .Include(b=>b.Subscriptions)
                .ThenInclude(c=>c.Client)
                .ToList();
            var subscriptionsForThatClient = _context.Subscriptions.ToList();
            int numSub = subscriptionsForThatClient.Count();
            int j = 0;

            //looping over the brokerages
            brokerages.ForEach(c =>
            {
                //for each brokerage going to create the object of BrokerageSubscriptionViewModel
                BrokerageSubscriptionsViewModel brokerageSubsViewModelObject = new BrokerageSubscriptionsViewModel
                {
                    BrokerageId = c.Id,
                    Title = c.Title,
                    IsMember = false    //while you are creating that object the isMember should be false
                };

                for (j = 0; j < c.Subscriptions.Count; j++)
                {
                    if (c.Subscriptions.ElementAt(j).ClientId == clientSubModel.Client.Id)
                    {
                        brokerageSubsViewModelObject.IsMember = true;
                        //clientSubModel.Subscriptions = clientSubModel.Subscriptions.Concat(new[] { brokerageSubsViewModelObject });
                    }


                }

                if (brokerageSubsViewModelObject.IsMember == true)
                {
                    clientSubModel.Subscriptions = clientSubModel.Subscriptions.Concat(new[] { brokerageSubsViewModelObject });
                }
                else
                {
                    nonSubscribers = nonSubscribers.Concat(new[] { brokerageSubsViewModelObject });
                }
            }
            );
            clientSubModel.Subscriptions = clientSubModel.Subscriptions.Concat(nonSubscribers);


            


            return View(clientSubModel);

        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Unregister(int ClientId, string BrokerageId)
        {

            Subscription sub = new Subscription();
            sub.ClientId = ClientId;
            sub.BrokerageId = BrokerageId;

            await _context.Subscriptions.Where(s => s.ClientId == ClientId && s.BrokerageId == BrokerageId).ToListAsync();
            _context.Subscriptions.Remove(sub);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditSubscriptions), new { id = ClientId });
        }


        public async Task<IActionResult> Register(int ClientId, string BrokerageId)
        {

            Subscription sub = new Subscription();
            sub.ClientId = ClientId;
            sub.BrokerageId = BrokerageId;

            _context.Subscriptions.Add(sub);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditSubscriptions), new { id = ClientId });
        }
    }
}
/*
 * 
 * 
 * */